using Aviscom.DTOs.Usuario;
using Aviscom.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NUlid;

namespace Aviscom.Controllers
{
 
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
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
        /// Busca uma lista de todos os usuários Pessoa Física.
        /// </summary>
        [HttpGet("pessoa-fisica")] // GET /api/usuarios/pessoa-fisica
        [ProducesResponseType(typeof(IEnumerable<UsuarioPessoaFisicaResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetUsuariosPessoaFisica()
        {
            try
            {
                var usuarios = await _usuarioService.GetUsuariosPessoaFisicaAsync();
                return Ok(usuarios);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro inesperado ao buscar lista de usuários PF.");
                return StatusCode(500, new { error = "Ocorreu um erro interno no servidor." });
            }
        }


        /// <summary>
        /// Busca um usuário Pessoa Física pelo ID.
        /// </summary>
        [HttpGet("pessoa-fisica/{id}")]
        [ProducesResponseType(typeof(UsuarioPessoaFisicaResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetPessoaFisicaById(Ulid id)
        {
            try
            {
                var usuario = await _usuarioService.GetPessoaFisicaByIdAsync(id);

                if (usuario == null)
                {
                    return NotFound(new { error = $"Usuário com ID {id} não encontrado." });
                }

                return Ok(usuario);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro inesperado ao buscar usuário PF pelo ID {UserId}.", id);
                return StatusCode(500, new { error = "Ocorreu um erro interno no servidor." });
            }
        }


        /// <summary>
        /// Atualiza parcialmente um usuário Pessoa Física.
        /// </summary>
        [HttpPatch("pessoa-fisica/{id}")]
        [ProducesResponseType(typeof(UsuarioPessoaFisicaResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdatePessoaFisica(Ulid id, [FromBody] UpdateUsuarioPessoaFisicaRequest request)
        {
            try
            {
                var usuario = await _usuarioService.UpdatePessoaFisicaAsync(id, request);

                if (usuario == null)
                {
                    return NotFound(new { error = $"Usuário com ID {id} não encontrado." });
                }

                return Ok(usuario);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro inesperado ao ATUALIZAR usuário PF pelo ID {UserId}.", id);
                return StatusCode(500, new { error = "Ocorreu um erro interno no servidor." });
            }
        }

        /// <summary>
        /// Exclui um usuário Pessoa Física.
        /// </summary>
        [HttpDelete("pessoa-fisica/{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeletePessoaFisica(Ulid id)
        {
            try
            {
                var foiExcluido = await _usuarioService.DeletePessoaFisicaAsync(id);

                if (!foiExcluido)
                {
                    return NotFound(new { error = $"Usuário com ID {id} não encontrado." });
                }

                // 204 No Content é a resposta padrão para um DELETE bem-sucedido
                // que não precisa retornar um corpo.
                return NoContent();
            }
            catch (Exception ex)
            {
                // ATENÇÃO: Se o usuário tiver dados relacionados (ex: Endereços)
                // e a regra de OnDelete não for 'Cascade', isso pode falhar
                // com uma DbUpdateException (conflito de chave estrangeira).
                _logger.LogError(ex, "Erro inesperado ao EXCLUIR usuário PF pelo ID {UserId}.", id);
                return StatusCode(500, new { error = "Ocorreu um erro interno no servidor." });
            }
        }
    }
}
