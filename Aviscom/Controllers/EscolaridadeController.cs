using Aviscom.DTOs.Usuario;
using Aviscom.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NUlid;

namespace Aviscom.Controllers
{
    [Route("api")]
    [ApiController]
    [Authorize]
    public class EscolaridadeController : ControllerBase
    {
        private readonly IEscolaridadeService _escolaridadeService;
        private readonly ILogger<EscolaridadeController> _logger;

        public EscolaridadeController(IEscolaridadeService escolaridadeService, ILogger<EscolaridadeController> logger)
        {
            _escolaridadeService = escolaridadeService;
            _logger = logger;
        }

        // === ENDPOINTS ANINHADOS (ASSOCIADOS AO UTILIZADOR) ===

        /// <summary>
        /// Cria um novo registo de escolaridade para um utilizador Pessoa Física. (Requer Login)
        /// </summary>
        [HttpPost("usuarios/pessoa-fisica/{usuarioPfId}/escolaridades")]
        [ProducesResponseType(typeof(EscolaridadeResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> CreateEscolaridadeParaPf([FromRoute] Ulid usuarioPfId, [FromBody] CreateEscolaridadeRequest request)
        {
            // TODO FUTURO: Adicionar verificação de segurança (se o utilizador logado
            // é o dono deste perfil ou é um Admin)

            try
            {
                var novaEscolaridade = await _escolaridadeService.CreateEscolaridadeParaPfAsync(usuarioPfId, request);

                return CreatedAtAction(
                    nameof(GetEscolaridadeById),
                    new { id = novaEscolaridade.Id },
                    novaEscolaridade);
            }
            catch (KeyNotFoundException ex) // Se o Utilizador ou Instituição não for encontrado
            {
                _logger.LogWarning("Falha ao criar escolaridade: {Message}", ex.Message);
                return NotFound(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro inesperado ao criar escolaridade para o utilizador {UsuarioId}", usuarioPfId);
                return StatusCode(500, new { error = "Ocorreu um erro interno no servidor." });
            }
        }

        /// <summary>
        /// Lista todos os registos de escolaridade de um utilizador Pessoa Física. (Requer Login)
        /// </summary>
        [HttpGet("usuarios/pessoa-fisica/{usuarioPfId}/escolaridades")]
        [ProducesResponseType(typeof(IEnumerable<EscolaridadeResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetEscolaridadesPessoaFisica([FromRoute] Ulid usuarioPfId)
        {
            // TODO FUTURO: Adicionar verificação de segurança

            try
            {
                var escolaridades = await _escolaridadeService.GetEscolaridadesByPfIdAsync(usuarioPfId);
                return Ok(escolaridades);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro inesperado ao buscar escolaridades do utilizador {UsuarioId}", usuarioPfId);
                return StatusCode(500, new { error = "Ocorreu um erro interno no servidor." });
            }
        }

        // === ENDPOINTS DIRETOS (PELO ID DA ESCOLARIDADE) ===

        /// <summary>
        /// Busca um registo de escolaridade pelo seu ID. (Requer Login)
        /// </summary>
        [HttpGet("escolaridades/{id}")]
        [ProducesResponseType(typeof(EscolaridadeResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetEscolaridadeById(Ulid id)
        {
            // TODO FUTURO: Adicionar verificação de segurança

            var escolaridade = await _escolaridadeService.GetEscolaridadeByIdAsync(id);
            if (escolaridade == null)
            {
                return NotFound(new { error = $"Registo de escolaridade com ID {id} não encontrado." });
            }
            return Ok(escolaridade);
        }

        /// <summary>
        /// Atualiza um registo de escolaridade existente. (Requer Login)
        /// </summary>
        [HttpPut("escolaridades/{id}")]
        [ProducesResponseType(typeof(EscolaridadeResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateEscolaridade(Ulid id, [FromBody] CreateEscolaridadeRequest request)
        {
            // TODO FUTURO: Adicionar verificação de segurança

            try
            {
                var escolaridade = await _escolaridadeService.UpdateEscolaridadeAsync(id, request);
                if (escolaridade == null)
                {
                    return NotFound(new { error = $"Registo de escolaridade com ID {id} não encontrado." });
                }
                return Ok(escolaridade);
            }
            catch (KeyNotFoundException ex) // Se a nova Instituição não for encontrada
            {
                _logger.LogWarning("Falha ao atualizar escolaridade: {Message}", ex.Message);
                return NotFound(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao atualizar escolaridade {Id}", id);
                return StatusCode(500, new { error = "Ocorreu um erro interno no servidor." });
            }
        }

        /// <summary>
        /// Exclui um registo de escolaridade. (Requer Login)
        /// </summary>
        [HttpDelete("escolaridades/{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteEscolaridade(Ulid id)
        {
            // TODO FUTURO: Adicionar verificação de segurança

            var sucesso = await _escolaridadeService.DeleteEscolaridadeAsync(id);
            if (!sucesso)
            {
                return NotFound(new { error = $"Registo de escolaridade com ID {id} não encontrado." });
            }
            return NoContent();
        }
    }
}
