using Aviscom.Dtos.Auth;
using Aviscom.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Aviscom.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IAuthService authService, ILogger<AuthController> logger)
        {
            _authService = authService;
            _logger = logger;
        }

        /// <summary>
        /// Autentica um usuário Pessoa Física e retorna um Token JWT.
        /// </summary>
        [HttpPost("login/pessoa-fisica")]
        [ProducesResponseType(typeof(LoginResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> LoginPessoaFisica([FromBody] LoginRequest request)
        {
            try
            {
                var response = await _authService.LoginPessoaFisicaAsync(request);

                if (response == null)
                {
                    return Unauthorized(new { error = "CPF ou senha inválidos." });
                }

                // Se o login for bem-sucedido, retorna o DTO de resposta
                // que contém o Token, ID, Nome e Funções
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro inesperado durante a tentativa de login.");
                return StatusCode(500, new { error = "Ocorreu um erro interno no servidor." });
            }
        }
    }
}
