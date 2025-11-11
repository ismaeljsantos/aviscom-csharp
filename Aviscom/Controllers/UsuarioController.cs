using Aviscom.DTOs.Usuario;
using Aviscom.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NUlid;

namespace Aviscom.Controllers
{
 
    [ApiController]
    [Route("api/[controller]")]
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioService _usuarioService;
        private readonly ILogger<UsuarioController> _logger;

        public UsuarioController(IUsuarioService usuarioService,
                                 ILogger<UsuarioController> logger)
        {
            _usuarioService = usuarioService;
            _logger = logger;
        }

        ///<summary>
        ///Cria um novo usuário do tipo Pessoa Física
        ///</summary>
        [HttpPost("pessoa-fisica")]
        [ProducesResponseType(typeof(UsuarioPessoaFisicaResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]

        public async Task<IActionResult> CreatePessoaFisica([FromBody] CreateUsuarioPessoaFisicaRequest request)
        {
            try
            {
                var novoUsuario = await _usuarioService.CreateUsuarioPessoaFisicaAsync(request);

                return CreatedAtAction(
                    nameof(GetPessoaFisicaById),
                    new { id = novoUsuario.Id },
                    novoUsuario
                    );
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Falha na criação do usuário PF (regra de negócio): {Message}", ex.Message);
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro inesperado ao criar usuario PF");
                return StatusCode(500, new { error = "Ocorreu um erro interno no servidor." });
            }
        }


        /// <summary>
        /// Busca um usuário Pessoa Física pelo ID. (Ainda não implementado)
        /// </summary>
        [HttpGet("pessoa-fisica/{id}")]
        [ProducesResponseType(typeof(UsuarioPessoaFisicaResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetPessoaFisicaById(Ulid id)
        {
            return Ok(new { Message = $"Endpoint 'Get' para o Id {id} ainda não implementado." });
        }
    }
}
