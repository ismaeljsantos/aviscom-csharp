using Aviscom.DTOs.Usuario;
using Aviscom.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NUlid;

namespace Aviscom.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class EnderecoController : ControllerBase
    {
        private readonly IEnderecoService _enderecoService;
        private readonly ILogger<EnderecoController> _logger;

        public EnderecoController(IEnderecoService enderecoService, ILogger<EnderecoController> logger)
        {
            _enderecoService = enderecoService;
            _logger = logger;
        }

        /// <summary>
        /// Cria um novo endereço para um usuário Pessoa Física.
        /// </summary>
        [HttpPost("usuario/pessoa-fisica/{usuarioPfId}/enderecos")]
        [ProducesResponseType(typeof(EnderecoResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateEnderecoParaPessoaFisica([FromRoute] Ulid usuarioPfId, [FromBody] CreateEnderecoRequest request)
        {
            try
            {
                var novoEndereco = await _enderecoService.CreateEnderecoParaPessoaFisicaAsync(usuarioPfId, request);

                return CreatedAtAction(
                    nameof(GetEnderecoById),
                    new { id = novoEndereco.Id },
                    novoEndereco);
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning("Falha ao criar endereço: {Message}", ex.Message);
                return NotFound(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro inesperado ao criar endereço para o usuário {UsuarioId}", usuarioPfId);
                return StatusCode(500, new { error = "Ocorreu um erro interno no servidor." });
            }
        }

        /// <summary>
        /// Lista todos os endereços de um usuário Pessoa Física.
        /// </summary>
        [HttpGet("usuarios/pessoa-fisica/{usuarioPfId}/enderecos")]
        [ProducesResponseType(typeof(IEnumerable<EnderecoResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetEnderecosPessoaFisica([FromRoute] Ulid usuarioPfId)
        {
            try
            {
                var enderecos = await _enderecoService.GetEnderecosByPessoaFisicaIdAsync(usuarioPfId);
                if (enderecos == null)
                {
                    return NotFound();
                }
                return Ok(enderecos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro inesperado ao buscar endereços do usuário {UsuarioId}", usuarioPfId);
                return StatusCode(500, new { error = "Ocorreu um erro interno no servidor." });
            }
        }

        /// <summary>
        /// Busca um endereço pelo seu ID.
        /// </summary>
        [HttpGet("enderecos/{id}")]
        [ProducesResponseType(typeof(EnderecoResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetEnderecoById(Ulid id)
        {
            try
            {
                var endereco = await _enderecoService.GetEnderecoByIdAsync(id);
                if (endereco == null)
                {
                    return NotFound(new { error = $"Endereço com ID {id} não encontrado." });
                }
                return Ok(endereco);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar Endereço {EnderecoId}", id);
                return StatusCode(500, new { error = "Ocorreu um erro interno no servidor." });
            }
        }

        /// <summary>
        /// Atualiza um endereço existente pelo seu ID.
        /// </summary>
        [HttpPut("enderecos/{id}")]
        [ProducesResponseType(typeof(EnderecoResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateEndereco(Ulid id, [FromBody] CreateEnderecoRequest request)
        {
            // Nota: Adicionaremos segurança aqui mais tarde (ex: verificar se o
            // usuário logado é dono deste endereço).
            try
            {
                var endereco = await _enderecoService.UpdateEnderecoAsync(id, request);
                if (endereco == null)
                {
                    return NotFound(new { error = $"Endereço com ID {id} não encontrado." });
                }
                return Ok(endereco);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao atualizar Endereço {EnderecoId}", id);
                return StatusCode(500, new { error = "Ocorreu um erro interno no servidor." });
            }
        }

        /// <summary>
        /// Exclui um endereço pelo seu ID.
        /// </summary>
        [HttpDelete("enderecos/{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteEndereco(Ulid id)
        {
            // Nota: Adicionaremos segurança aqui mais tarde.
            try
            {
                var sucesso = await _enderecoService.DeleteEnderecoAsync(id);
                if (!sucesso)
                {
                    return NotFound(new { error = $"Endereço com ID {id} não encontrado." });
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao excluir Endereço {EnderecoId}", id);
                return StatusCode(500, new { error = "Ocorreu um erro interno no servidor." });
            }

        }
    }
}
