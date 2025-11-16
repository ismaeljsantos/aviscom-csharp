using Aviscom.Dtos.Auth;
using Aviscom.DTOs.Auth;
using Aviscom.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
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
        /// Autentica um utilizador Pessoa Física e retorna um Token JWT.
        /// </summary>
        [HttpPost("login/pessoa-fisica")]
        [AllowAnonymous] // Este já deve existir
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

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro inesperado durante a tentativa de login de PF.");
                return StatusCode(500, new { error = "Ocorreu um erro interno no servidor." });
            }
        }


        /// <summary>
        /// Autentica um utilizador Pessoa Jurídica e retorna um Token JWT.
        /// </summary>
        [HttpPost("login/pessoa-juridica")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(LoginResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> LoginPessoaJuridica([FromBody] LoginPjRequest request)
        {
            try
            {
                var response = await _authService.LoginPessoaJuridicaAsync(request);

                if (response == null)
                {
                    return Unauthorized(new { error = "CNPJ ou senha inválidos." });
                }

                // O LoginResponse é o mesmo, mas o 'Nome' será a RazaoSocial
                // e as 'Funcoes' virão da associação de PJ
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro inesperado durante a tentativa de login de PJ.");
                return StatusCode(500, new { error = "Ocorreu um erro interno no servidor." });
            }
        }
    }
}
