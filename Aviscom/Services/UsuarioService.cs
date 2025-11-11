using Aviscom.Data;
using Aviscom.DTOs.Usuario;
using Aviscom.Models.Usuario;
using Aviscom.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
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
                CpfCriptografado = cpfCriptografado, //
                CpfHash = cpfHash, //
                SenhaHash = senhaHash //
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

        //private string CriptografarCpfPlaceholder(string cpfLimpo)
        //{
        //    // !! RISCO DE SEGURANÇA !!
        //    // Isto NÃO é criptografia. É apenas um placeholder.
        //    // A implementação real (ex: AES) é complexa e exige
        //    // gerenciamento de chaves de criptografia.
        //    _logger.LogWarning("CRIPTOGRAFIA DE CPF NÃO IMPLEMENTADA. Usando Base64 como placeholder. ISSO DEVE SER CORRIGIDO.");

        //    // Apenas para simular que algo foi feito, vamos inverter e usar Base64
        //    var bytes = Encoding.UTF8.GetBytes(cpfLimpo);
        //    return Convert.ToBase64String(bytes);
        //}
    }
}
