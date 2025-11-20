using Aviscom.Data;
using Aviscom.Dtos.Auth; // Assumo que o LoginRequest está aqui
using Aviscom.Models.Usuario; // Assumo que UsuarioPessoaFisica está aqui
using Aviscom.Services;
using Aviscom.Services.Interfaces;
using Microsoft.EntityFrameworkCore; // NECESSÁRIO
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Aviscom.Tests
{
    public class AuthServiceTests
    {
        private readonly AuthService _authService;

        // CORRIGIDO: Agora _mockContext é um mock de IQueryable/IDbSet, não do Context
        private readonly Mock<ITokenService> _mockTokenService;
        private readonly Mock<ILogger<AuthService>> _mockLogger;

        // O DbContext real será inicializado in-memory para simular as queries
        private readonly AviscomContext _context;

        public AuthServiceTests()
        {
            // 1. Configurar o DbContext In-Memory para testes
            var options = new DbContextOptionsBuilder<AviscomContext>()
                .UseInMemoryDatabase(databaseName: "TestDb_" + Guid.NewGuid().ToString())
                .Options;

            _context = new AviscomContext(options);
            _context.Database.EnsureDeleted(); // Garante um banco limpo a cada teste

            // 2. Inicializar os mocks
            _mockTokenService = new Mock<ITokenService>();
            _mockLogger = new Mock<ILogger<AuthService>>();

            // 3. Injetar o Context REAL no serviço (para simular a persistência de forma leve)
            _authService = new AuthService(
                _context, // Passamos o Context In-Memory real
                _mockTokenService.Object,
                _mockLogger.Object
            );
        }

        [Fact]
        public async Task LoginPessoaFisicaAsync_DeveRetornarNull_QuandoCPFNaoExiste()
        {
            // ARRANGE (Garantir que o Context está vazio, já feito no construtor)
            var request = new LoginRequest { Cpf = "11122233344", Senha = "Senha" };

            // ACT (Ação)
            var resultado = await _authService.LoginPessoaFisicaAsync(request);

            // ASSERT (Verificação)
            // Se o Context está vazio, o login deve falhar
            Assert.Null(resultado);
        }
    }
}
