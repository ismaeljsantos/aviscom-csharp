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
    public class ExperienciaProfissionalController : ControllerBase
    {
        private readonly IExperienciaProfissionalService _experienciaService;
        private readonly ILogger<ExperienciaProfissionalController> _logger;

        public ExperienciaProfissionalController(IExperienciaProfissionalService experienciaService, ILogger<ExperienciaProfissionalController> logger)
        {
            _experienciaService = experienciaService;
            _logger = logger;
        }

        // === ENDPOINTS ANINHADOS (ASSOCIADOS AO UTILIZADOR) ===

        /// <summary>
        /// Cria um novo registo de experiência profissional para um utilizador PF. (Requer Login)
        /// </summary>
        [HttpPost("usuarios/pessoa-fisica/{usuarioPfId}/experiencias")]
        [ProducesResponseType(typeof(ExperienciaProfissionalResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> CreateExperienciaParaPf([FromRoute] Ulid usuarioPfId, [FromBody] CreateExperienciaProfissionalRequest request)
        {
            // TODO FUTURO: Adicionar verificação de segurança (se o utilizador logado
            // é o dono deste perfil ou é um Admin)

            try
            {
                var novaExperiencia = await _experienciaService.CreateExperienciaParaPfAsync(usuarioPfId, request);

                return CreatedAtAction(
                    nameof(GetExperienciaById),
                    new { id = novaExperiencia.Id },
                    novaExperiencia);
            }
            catch (KeyNotFoundException ex) // Se o Utilizador ou Empresa não for encontrado
            {
                _logger.LogWarning("Falha ao criar experiência: {Message}", ex.Message);
                return NotFound(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro inesperado ao criar experiência para o utilizador {UsuarioId}", usuarioPfId);
                return StatusCode(500, new { error = "Ocorreu um erro interno no servidor." });
            }
        }

        /// <summary>
        /// Lista todos os registos de experiência profissional de um utilizador PF. (Requer Login)
        /// </summary>
        [HttpGet("usuarios/pessoa-fisica/{usuarioPfId}/experiencias")]
        [ProducesResponseType(typeof(IEnumerable<ExperienciaProfissionalResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetExperienciasPessoaFisica([FromRoute] Ulid usuarioPfId)
        {
            // TODO FUTURO: Adicionar verificação de segurança

            try
            {
                var experiencias = await _experienciaService.GetExperienciasByPfIdAsync(usuarioPfId);
                return Ok(experiencias);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro inesperado ao buscar experiências do utilizador {UsuarioId}", usuarioPfId);
                return StatusCode(500, new { error = "Ocorreu um erro interno no servidor." });
            }
        }

        // === ENDPOINTS DIRETOS (PELO ID DA EXPERIÊNCIA) ===

        /// <summary>
        /// Busca um registo de experiência profissional pelo seu ID. (Requer Login)
        /// </summary>
        [HttpGet("experiencias/{id}")]
        [ProducesResponseType(typeof(ExperienciaProfissionalResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetExperienciaById(Ulid id)
        {
            // TODO FUTURO: Adicionar verificação de segurança

            var experiencia = await _experienciaService.GetExperienciaByIdAsync(id);
            if (experiencia == null)
            {
                return NotFound(new { error = $"Registo de experiência com ID {id} não encontrado." });
            }
            return Ok(experiencia);
        }

        /// <summary>
        /// Atualiza um registo de experiência profissional existente. (Requer Login)
        /// </summary>
        [HttpPut("experiencias/{id}")]
        [ProducesResponseType(typeof(ExperienciaProfissionalResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateExperiencia(Ulid id, [FromBody] CreateExperienciaProfissionalRequest request)
        {
            // TODO FUTURO: Adicionar verificação de segurança

            try
            {
                var experiencia = await _experienciaService.UpdateExperienciaAsync(id, request);
                if (experiencia == null)
                {
                    return NotFound(new { error = $"Registo de experiência com ID {id} não encontrado." });
                }
                return Ok(experiencia);
            }
            catch (KeyNotFoundException ex) // Se a nova Empresa não for encontrada
            {
                _logger.LogWarning("Falha ao atualizar experiência: {Message}", ex.Message);
                return NotFound(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao atualizar experiência {Id}", id);
                return StatusCode(500, new { error = "Ocorreu um erro interno no servidor." });
            }
        }

        /// <summary>
        /// Exclui um registo de experiência profissional. (Requer Login)
        /// </summary>
        [HttpDelete("experiencias/{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteExperiencia(Ulid id)
        {
            // TODO FUTURO: Adicionar verificação de segurança

            var sucesso = await _experienciaService.DeleteExperienciaAsync(id);
            if (!sucesso)
            {
                return NotFound(new { error = $"Registo de experiência com ID {id} não encontrado." });
            }
            return NoContent();
        }
    }
}
