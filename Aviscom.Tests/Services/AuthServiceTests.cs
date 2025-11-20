using Aviscom.Data;
using Aviscom.Dtos.Auth;
using Aviscom.Models.Usuario;
using Aviscom.Services;
using Aviscom.Services.Interfaces;
using BCrypt.Net; // NECESSÁRIO para criar o hash da senha
using static BCrypt.Net.BCrypt;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Security.Cryptography;
using System.Text;
using Xunit;

namespace Aviscom.Tests.Services
{
    public class AuthServiceTests : IDisposable // Implementa IDisposable para limpar o DB
    {
        private readonly AuthService _authService;
        private readonly Mock<ITokenService> _mockTokenService;
        private readonly AviscomContext _context;

        // Dados de teste
        private const string CpfTeste = "12345678910";
        private const string SenhaTeste = "SenhaSegura123!";
        private const string NomeUsuarioTeste = "Ana Maria Braga";

        public AuthServiceTests()
        {
            // Configuração do DbContext In-Memory
            var options = new DbContextOptionsBuilder<AviscomContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new AviscomContext(options);
            _context.Database.EnsureCreated(); // Garante que o DB in-memory está criado

            // Inicialização dos mocks
            _mockTokenService = new Mock<ITokenService>();
            var mockLogger = new Mock<ILogger<AuthService>>();

            // 1. Configurar o Mock do TokenService para retornar um token falso (simulado)
            _mockTokenService.Setup(t => t.GenerateToken(
                It.IsAny<UsuarioPessoaFisica>(),
                It.IsAny<List<string>>()
            )).Returns("TOKEN_JWT_SIMULADO");

            // Injetar o Context REAL e o Mock do TokenService
            _authService = new AuthService(
                _context,
                _mockTokenService.Object,
                mockLogger.Object
            );
        }

        // Garante que o banco de dados é limpo após cada teste
        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        private string GerarHashSHA256(string input)
        {
            using var sha256 = SHA256.Create();
            var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(input));
            return Convert.ToBase64String(bytes);
        }

        // --- TESTES ---

        [Fact]
        public async Task LoginPessoaFisicaAsync_DeveRetornarNull_QuandoCPFNaoExiste()
        {
            // Teste de falha (já implementado)
            var request = new LoginRequest { Cpf = CpfTeste, Senha = SenhaTeste };
            var resultado = await _authService.LoginPessoaFisicaAsync(request);
            Assert.Null(resultado);
        }

        [Fact]
        public async Task LoginPessoaFisicaAsync_DeveRetornarToken_QuandoCredenciaisValidas()
        {
            // ARRANGE (Preparação dos dados no DB)
            var cpfLimpo = CpfTeste;
            var cpfHash = GerarHashSHA256(cpfLimpo);
            var senhaHash = BCrypt.Net.BCrypt.HashPassword(SenhaTeste); // Hash real

            var usuarioTeste = new UsuarioPessoaFisica
            {
                Nome = NomeUsuarioTeste,
                CpfCriptografado = "NaoImportaParaLogin",
                CpfHash = cpfHash,
                SenhaHash = senhaHash,
                DataNascimento = DateTime.UtcNow.AddYears(-30)
            };

            await _context.UsuariosFisicos.AddAsync(usuarioTeste);
            await _context.SaveChangesAsync();

            var request = new LoginRequest { Cpf = CpfTeste, Senha = SenhaTeste };

            // ACT (Ação)
            var resultado = await _authService.LoginPessoaFisicaAsync(request);

            // ASSERT (Verificação)
            Assert.NotNull(resultado);
            Assert.Equal("TOKEN_JWT_SIMULADO", resultado.Token);
            Assert.Equal(NomeUsuarioTeste, resultado.Nome);
            Assert.Equal(usuarioTeste.Id, resultado.UsuarioId);
        }

        [Fact]
        public async Task LoginPessoaFisicaAsync_DeveRetornarNull_QuandoSenhaInvalida()
        {
            // ARRANGE (Preparação)
            var cpfLimpo = CpfTeste;
            var cpfHash = GerarHashSHA256(cpfLimpo);
            var senhaHash = BCrypt.Net.BCrypt.HashPassword(SenhaTeste); // Hash da senha correta

            var usuarioTeste = new UsuarioPessoaFisica
            {
                Nome = NomeUsuarioTeste,
                CpfCriptografado = "NaoImportaParaLogin",
                CpfHash = cpfHash,
                SenhaHash = senhaHash,
                DataNascimento = DateTime.UtcNow.AddYears(-30)
            };

            await _context.UsuariosFisicos.AddAsync(usuarioTeste);
            await _context.SaveChangesAsync();

            var request = new LoginRequest { Cpf = CpfTeste, Senha = "SenhaTotalmenteErrada" }; // Senha errada

            // ACT (Ação)
            var resultado = await _authService.LoginPessoaFisicaAsync(request);

            // ASSERT (Verificação)
            Assert.Null(resultado);
        }
    }
}