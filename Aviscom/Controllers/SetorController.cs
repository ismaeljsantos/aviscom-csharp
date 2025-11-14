using Aviscom.DTOs.Admin;
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
    public class SetorController : ControllerBase
    {
        private readonly ISetorService _setorService;
        private readonly ILogger<SetorController> _logger;

        public SetorController(ISetorService setorService, ILogger<SetorController> logger)
        {
            _setorService = setorService;
            _logger = logger;
        }

        /// <summary>
        /// Cria um novo setor. (Requer Admin)
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(SetorResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateSetor([FromBody] CreateSetorRequest request)
        {
            try
            {
                var novoSetor = await _setorService.CreateSetorAsync(request);
                return CreatedAtAction(nameof(GetSetorById), new { id = novoSetor.Id }, novoSetor);
            }
            catch (InvalidOperationException ex) // Erro de "já existe"
            {
                _logger.LogWarning(ex, "Tentativa de criar setor duplicado: {Nome}", request.Nome);
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao criar setor.");
                return StatusCode(500, new { error = "Ocorreu um erro interno no servidor." });
            }
        }

        /// <summary>
        /// Lista todos os setores do sistema. (Requer Admin)
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<SetorResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSetores()
        {
            var setores = await _setorService.GetSetoresAsync();
            return Ok(setores);
        }

        /// <summary>
        /// Busca um setor pelo ID. (Requer Admin)
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(SetorResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetSetorById(Ulid id)
        {
            var setor = await _setorService.GetSetorByIdAsync(id);
            if (setor == null)
            {
                return NotFound(new { error = $"Setor com ID {id} não encontrado." });
            }
            return Ok(setor);
        }

        /// <summary>
        /// Atualiza um setor existente. (Requer Admin)
        /// </summary>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(SetorResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateSetor(Ulid id, [FromBody] CreateSetorRequest request)
        {
            var setor = await _setorService.UpdateSetorAsync(id, request);
            if (setor == null)
            {
                return NotFound(new { error = $"Setor com ID {id} não encontrado." });
            }
            return Ok(setor);
        }

        /// <summary>
        /// Exclui um setor. (Requer Admin)
        /// </summary>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteSetor(Ulid id)
        {
            var sucesso = await _setorService.DeleteSetorAsync(id);
            if (!sucesso)
            {
                return NotFound(new { error = $"Setor com ID {id} não encontrado." });
            }
            return NoContent();
        }
    }
}
