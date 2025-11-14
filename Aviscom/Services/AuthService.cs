using Aviscom.Data;
using Aviscom.Dtos.Auth;
using Aviscom.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace Aviscom.Services
{
    public class AuthService : IAuthService
    {
        private readonly AviscomContext _context;
        private readonly ITokenService _tokenService;
        private readonly ILogger<AuthService> _logger;

        public AuthService(AviscomContext context, ITokenService tokenService, ILogger<AuthService> logger)
        {
            _context = context;
            _tokenService = tokenService;
            _logger = logger;
        }

        public async Task<LoginResponse?> LoginPessoaFisicaAsync(LoginRequest request)
        {
            try
            {
                // 1. Limpar e hashear o CPF (mesma lógica do cadastro)
                var cpfLimpo = LimparCpf(request.Cpf);
                var cpfHash = GerarHashSHA256(cpfLimpo);

                // 2. Encontrar o usuário pelo CpfHash
                var usuario = await _context.UsuariosFisicos
                                    .AsNoTracking()
                                    .FirstOrDefaultAsync(u => u.CpfHash == cpfHash);

                if (usuario == null)
                {
                    _logger.LogWarning("Tentativa de login falhou: CPF não encontrado (Hash: {CpfHash})", cpfHash);
                    return null; // Usuário não encontrado
                }

                // 3. Verificar a Senha
                bool senhaValida = BCrypt.Net.BCrypt.Verify(request.Senha, usuario.SenhaHash);

                if (!senhaValida)
                {
                    _logger.LogWarning("Tentativa de login falhou: Senha inválida para usuário {UsuarioId}", usuario.Id);
                    return null; // Senha incorreta
                }

                // 4. (IMPORTANTE) Buscar as Funções (Roles) do usuário
                var funcoes = await _context.UsuariosFuncoes
                    .Where(uf => uf.FkPessoaFisicaId == usuario.Id) //
                    .Include(uf => uf.Funcao) //
                    .Select(uf => uf.Funcao.Titulo)
                    .ToListAsync();

                // 5. Gerar o Token
                var tokenString = _tokenService.GenerateToken(usuario, funcoes);

                // 6. Retornar a Resposta
                return new LoginResponse
                {
                    Token = tokenString,
                    Expiration = DateTime.UtcNow.AddHours(8), // Deve ser o mesmo do TokenService
                    UsuarioId = usuario.Id,
                    Nome = usuario.Nome,
                    Funcoes = funcoes
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro inesperado durante o processo de login para CPF (Hash: {CpfHash})", GerarHashSHA256(LimparCpf(request.Cpf)));
                return null;
            }
        }

        // --- Métodos Auxiliares (copiados do UsuarioService) ---
        private string LimparCpf(string cpf)
        {
            return Regex.Replace(cpf ?? "", @"[^\d]", "");
        }

        private string GerarHashSHA256(string input)
        {
            using var sha256 = SHA256.Create();
            var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(input));
            return Convert.ToBase64String(bytes);
        }
    }
}