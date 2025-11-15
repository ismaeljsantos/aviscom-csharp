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
    [Authorize(Policy = "Administrador")]
    public class InstituicaoController : ControllerBase
    {
        private readonly IInstituicaoService _instituicaoService;
        private readonly ILogger<InstituicaoController> _logger;

        public InstituicaoController(IInstituicaoService instituicaoService, ILogger<InstituicaoController> logger)
        {
            _instituicaoService = instituicaoService;
            _logger = logger;
        }

        /// <summary>
        /// Cria uma nova instituição de ensino. (Requer Admin)
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(InstituicaoResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateInstituicao([FromBody] CreateInstituicaoRequest request)
        {
            try
            {
                var novaInstituicao = await _instituicaoService.CreateInstituicaoAsync(request);
                return CreatedAtAction(nameof(GetInstituicaoById), new { id = novaInstituicao.Id }, novaInstituicao);
            }
            catch (InvalidOperationException ex) // Erro de "já existe"
            {
                _logger.LogWarning(ex, "Tentativa de criar instituição duplicada: {Nome}", request.Nome);
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao criar instituição.");
                return StatusCode(500, new { error = "Ocorreu um erro interno no servidor." });
            }
        }

        /// <summary>
        /// Lista todas as instituições de ensino do sistema. (Requer Admin)
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<InstituicaoResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetInstituicoes()
        {
            var instituicoes = await _instituicaoService.GetInstituicoesAsync();
            return Ok(instituicoes);
        }

        /// <summary>
        /// Busca uma instituição pelo ID. (Requer Admin)
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(InstituicaoResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetInstituicaoById(Ulid id)
        {
            var instituicao = await _instituicaoService.GetInstituicaoByIdAsync(id);
            if (instituicao == null)
            {
                return NotFound(new { error = $"Instituição com ID {id} não encontrada." });
            }
            return Ok(instituicao);
        }

        /// <summary>
        /// Atualiza uma instituição existente. (Requer Admin)
        /// </summary>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(InstituicaoResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateInstituicao(Ulid id, [FromBody] CreateInstituicaoRequest request)
        {
            var instituicao = await _instituicaoService.UpdateInstituicaoAsync(id, request);
            if (instituicao == null)
            {
                return NotFound(new { error = $"Instituição com ID {id} não encontrada." });
            }
            return Ok(instituicao);
        }

        /// <summary>
        /// Exclui uma instituição. (Requer Admin)
        /// </summary>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteInstituicao(Ulid id)
        {
            var sucesso = await _instituicaoService.DeleteInstituicaoAsync(id);
            if (!sucesso)
            {
                return NotFound(new { error = $"Instituição com ID {id} não encontrada." });
            }
            return NoContent();
        }
    }
}
