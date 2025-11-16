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

        public async Task<bool> DeletePessoaFisicaAsync(Ulid id)
        {
            var usuario = await _context.UsuariosFisicos.FindAsync(id);

            if(usuario == null)
            { 
                return false;
            }

            _context.UsuariosFisicos.Remove(usuario);
            await _context.SaveChangesAsync();

            return true;
        }

        //=========================
        // ====== MÉTODOS PJ ======
        //=========================

        public async Task<UsuarioPessoaJuridicaResponse> CreateUsuarioPessoaJuridicaAsync(CreateUsuarioPessoaJuridicaRequest request)
        {
            // 1. Validar o Responsável (Pessoa Física)
            var responsavel = await _context.UsuariosFisicos.FindAsync(request.FkResponsavelId);
            if (responsavel == null)
            {
                throw new KeyNotFoundException($"Usuário responsável (Pessoa Física) com ID {request.FkResponsavelId} não encontrado.");
            }

            // 2. Limpar e Hashear o CNPJ
            var cnpjLimpo = LimparCnpj(request.Cnpj);
            var cnpjHash = GerarHashSHA256(cnpjLimpo);

            // 3. Verificar Duplicatas de CNPJ
            if (await _context.UsuariosJuridicos.AnyAsync(u => u.CnpjHash == cnpjHash))
            {
                throw new InvalidOperationException("Uma empresa com este CNPJ já existe.");
            }

            // 4. Hashear a Senha
            var senhaHash = BCrypt.Net.BCrypt.HashPassword(request.Senha);

            // 5. Mapear DTO para Entidade
            var novoUsuarioPJ = new UsuarioPessoaJuridica
            {
                RazaoSocial = request.RazaoSocial,
                NomeFantasia = request.NomeFantasia,
                Cnpj = cnpjLimpo, // Salva o CNPJ limpo
                CnpjHash = cnpjHash,
                SenhaHash = senhaHash,
                FkResponsavelId = request.FkResponsavelId
            };

            // 6. Salvar
            await _context.UsuariosJuridicos.AddAsync(novoUsuarioPJ);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Novo usuário PJ criado com Id: {UserId}", novoUsuarioPJ.Id);

            // 7. Retornar DTO de Resposta
            return new UsuarioPessoaJuridicaResponse
            {
                Id = novoUsuarioPJ.Id,
                RazaoSocial = novoUsuarioPJ.RazaoSocial,
                NomeFantasia = novoUsuarioPJ.NomeFantasia,
                Cnpj = cnpjLimpo, // (Podemos formatar isto no DTO depois, se quiser)
                DataCriacao = novoUsuarioPJ.DataCriacao,
                FkResponsavelId = novoUsuarioPJ.FkResponsavelId,
                NomeResponsavel = responsavel.Nome
            };
        }

        public async Task<IEnumerable<UsuarioPessoaJuridicaResponse>> GetUsuariosPessoaJuridicaAsync()
        {
            // Usamos .Select() para projetar diretamente para o DTO
            return await _context.UsuariosJuridicos
                .AsNoTracking()
                .Include(u => u.Responsavel) // Inclui o Responsável (PF)
                .Select(u => new UsuarioPessoaJuridicaResponse
                {
                    Id = u.Id,
                    RazaoSocial = u.RazaoSocial,
                    NomeFantasia = u.NomeFantasia,
                    Cnpj = u.Cnpj, // (Podemos formatar isto no DTO de Resposta depois)
                    DataCriacao = u.DataCriacao,
                    FkResponsavelId = u.FkResponsavelId,
                    NomeResponsavel = u.Responsavel.Nome
                })
                .ToListAsync();
        }

        public async Task<UsuarioPessoaJuridicaResponse?> GetPessoaJuridicaByIdAsync(Ulid id)
        {
            return await _context.UsuariosJuridicos
                .AsNoTracking()
                .Where(u => u.Id == id)
                .Include(u => u.Responsavel) //
                .Select(u => new UsuarioPessoaJuridicaResponse
                {
                    Id = u.Id,
                    RazaoSocial = u.RazaoSocial,
                    NomeFantasia = u.NomeFantasia,
                    Cnpj = u.Cnpj,
                    DataCriacao = u.DataCriacao,
                    FkResponsavelId = u.FkResponsavelId,
                    NomeResponsavel = u.Responsavel.Nome
                })
                .FirstOrDefaultAsync();
        }

        public async Task<UsuarioPessoaJuridicaResponse?> UpdatePessoaJuridicaAsync(Ulid id, UpdateUsuarioPessoaJuridicaRequest request)
        {
            var usuarioPJ = await _context.UsuariosJuridicos
                                .Include(u => u.Responsavel) // Carrega o responsável atual
                                .FirstOrDefaultAsync(u => u.Id == id);

            if (usuarioPJ == null)
            {
                return null; // Não encontrado
            }

            // 1. Atualiza campos simples
            if (!string.IsNullOrWhiteSpace(request.RazaoSocial))
            {
                usuarioPJ.RazaoSocial = request.RazaoSocial;
            }
            if (request.NomeFantasia != null)
            {
                usuarioPJ.NomeFantasia = request.NomeFantasia;
            }

            var nomeResponsavel = usuarioPJ.Responsavel.Nome;

            // 2. Atualiza o Responsável (se for fornecido e for diferente)
            if (request.FkResponsavelId.HasValue && request.FkResponsavelId.Value != usuarioPJ.FkResponsavelId)
            {
                var novoResponsavel = await _context.UsuariosFisicos.FindAsync(request.FkResponsavelId.Value);
                if (novoResponsavel == null)
                {
                    throw new KeyNotFoundException($"Novo usuário responsável (Pessoa Física) com ID {request.FkResponsavelId.Value} não encontrado.");
                }

                usuarioPJ.FkResponsavelId = request.FkResponsavelId.Value;
                nomeResponsavel = novoResponsavel.Nome; // Atualiza o nome para o DTO de resposta
            }

            // 3. Salva
            await _context.SaveChangesAsync();

            _logger.LogInformation("Utilizador PJ {UserId} atualizado.", id);

            // 4. Retorna o DTO de Resposta
            return MapearParaPjResponse(usuarioPJ, nomeResponsavel);
        }

        public async Task<bool> DeletePessoaJuridicaAsync(Ulid id)
        {
            var usuarioPJ = await _context.UsuariosJuridicos.FindAsync(id);
            if (usuarioPJ == null)
            {
                return false; // Não encontrado
            }

            // Lógica de exclusão em cascata (se necessária)
            // (Conforme discutimos, o padrão atual para Endereço/Contato
            // é 'ClientSetNull', então eles ficarão órfãos, o que é aceitável por agora)

            _context.UsuariosJuridicos.Remove(usuarioPJ);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Utilizador PJ {UserId} excluído.", id);
            return true;
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

        private string LimparCnpj(string cnpj)
        {
            // Remove tudo que não for dígito
            return Regex.Replace(cnpj ?? "", @"[^\d]", "");
        }

        private UsuarioPessoaJuridicaResponse MapearParaPjResponse(UsuarioPessoaJuridica u, string nomeResponsavel)
        {
            return new UsuarioPessoaJuridicaResponse
            {
                Id = u.Id,
                RazaoSocial = u.RazaoSocial,
                NomeFantasia = u.NomeFantasia,
                Cnpj = u.Cnpj,
                DataCriacao = u.DataCriacao,
                FkResponsavelId = u.FkResponsavelId,
                NomeResponsavel = nomeResponsavel
            };
        }

    }
}
