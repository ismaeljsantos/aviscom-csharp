using Aviscom.Data;
using Aviscom.DTOs.Usuario;
using Aviscom.Models.Usuario;
using Aviscom.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using NUlid;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace Aviscom.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly AviscomContext _context;
        private readonly ILogger<UsuarioService> _logger;
        private readonly IEncryptionService _encryptionService;

        public UsuarioService(AviscomContext context, 
                              ILogger<UsuarioService> logger,
                              IEncryptionService encryptionService)
        {
            _context = context;
            _logger = logger;
            _encryptionService = encryptionService;
        }

        public async Task<IEnumerable<UsuarioPessoaFisicaResponse>> GetUsuariosPessoaFisicaAsync()
        {
            var usuarios = await _context.UsuariosFisicos
                .AsNoTracking()
                .Select(u => new UsuarioPessoaFisicaResponse
                { 
                    Id = u.Id,
                    Nome = u.Nome,
                    NomeSocial = u.NomeSocial,
                    Sexo = u.Sexo,
                    DataNascimento = u.DataNascimento,
                    NomeMae = u.NomeMae,
                    NomePai = u.NomePai,
                    DataCriacao = u.DataCriacao
                }).ToListAsync();

            return usuarios;

        }

        public async Task<UsuarioPessoaFisicaResponse?> GetPessoaFisicaByIdAsync(Ulid id)
        {

            var usuario = await _context.UsuariosFisicos 
                .AsNoTracking()
                .Where(u => u.Id == id) 
                .Select(u => new UsuarioPessoaFisicaResponse
                {
                    Id = u.Id,
                    Nome = u.Nome,
                    NomeSocial = u.NomeSocial,
                    Sexo = u.Sexo,
                    DataNascimento = u.DataNascimento,
                    NomeMae = u.NomeMae,
                    NomePai = u.NomePai,
                    DataCriacao = u.DataCriacao
                })
                .FirstOrDefaultAsync();

            return usuario;
        }

        public async Task<UsuarioPessoaFisicaResponse> CreateUsuarioPessoaFisicaAsync(CreateUsuarioPessoaFisicaRequest request)
        {
            var cpfLimpo = LimparCpf(request.Cpf);

            var cpfHash = GerarHashSHA256(cpfLimpo);

            if (await _context.UsuariosFisicos.AnyAsync(u => u.CpfHash == cpfHash))
            {
                throw new InvalidOperationException("Já existe um usuário com este CPF.");
            }

            var senhaHash = BCrypt.Net.BCrypt.HashPassword(request.Senha);
            var cpfCriptografado = _encryptionService.Encrypt(cpfLimpo);

            var novoUsuario = new UsuarioPessoaFisica
            {
                Nome = request.Nome,
                NomeSocial = request.NomeSocial,
                Sexo = request.Sexo,
                DataNascimento = request.DataNascimento,
                NomeMae = request.NomeMae,
                NomePai = request.NomePai,
                CpfCriptografado = cpfCriptografado, 
                CpfHash = cpfHash, 
                SenhaHash = senhaHash 
                // Id, DataCriacao, DataAtualizacao são preenchidos pela BaseEntity e Context
            };

            await _context.UsuariosFisicos.AddAsync(novoUsuario);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Novo usuário PF criado com Id: {UserId}", novoUsuario.Id);
            
            return new UsuarioPessoaFisicaResponse
            {
                Id = novoUsuario.Id,
                Nome = novoUsuario.Nome,
                NomeSocial = novoUsuario.NomeSocial,
                Sexo = novoUsuario.Sexo,
                DataNascimento = novoUsuario.DataNascimento,
                NomeMae = novoUsuario.NomeMae,
                NomePai = novoUsuario.NomePai,
                DataCriacao = novoUsuario.DataCriacao
            };
        }

        public async Task<UsuarioPessoaFisicaResponse?> UpdatePessoaFisicaAsync(Ulid id, UpdateUsuarioPessoaFisicaRequest request)
        {
            var usuario = await _context.UsuariosFisicos.FindAsync(id);

            if (usuario == null)
            {
                return null;
            }
            if (!string.IsNullOrWhiteSpace(request.Nome))
            {
                usuario.Nome = request.Nome;
            }
            if (request.NomeSocial != null)
            {
                usuario.NomeSocial = request.NomeSocial;
            }
            if (request.Sexo != null)
            {
                usuario.Sexo = request.Sexo;
            }
            if (request.DataNascimento.HasValue) 
            {
                usuario.DataNascimento = request.DataNascimento.Value;
            }
            if (request.NomeMae != null)
            {
                usuario.NomeMae = request.NomeMae;
            }
            if (request.NomePai != null)
            {
                usuario.NomePai = request.NomePai;
            }

            await _context.SaveChangesAsync();

            return new UsuarioPessoaFisicaResponse
            {
                Id = usuario.Id,
                Nome = usuario.Nome,
                NomeSocial = usuario.NomeSocial,
                Sexo = usuario.Sexo,
                DataNascimento = usuario.DataNascimento,
                NomeMae = usuario.NomeMae,
                NomePai = usuario.NomePai,
                DataCriacao = usuario.DataCriacao
            };
        }

        private string LimparCpf(string cpf)
        {
            return Regex.Replace(cpf, @"[^\d]", "");
        }

        private string GerarHashSHA256(string input)
        {
            using var sha256 = SHA256.Create();
            var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(input));
            return Convert.ToBase64String(bytes);
        }

    }
}
