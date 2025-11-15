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
    public class EmpresaController : ControllerBase
    {
        private readonly IEmpresaService _empresaService;
        private readonly ILogger<EmpresaController> _logger;

        public EmpresaController(IEmpresaService empresaService, ILogger<EmpresaController> logger)
        {
            _empresaService = empresaService;
            _logger = logger;
        }

        /// <summary>
        /// Cria uma nova empresa. (Requer Admin)
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(EmpresaResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateEmpresa([FromBody] CreateEmpresaRequest request)
        {
            try
            {
                var novaEmpresa = await _empresaService.CreateEmpresaAsync(request);
                return CreatedAtAction(nameof(GetEmpresaById), new { id = novaEmpresa.Id }, novaEmpresa);
            }
            catch (InvalidOperationException ex) // Erro de "já existe"
            {
                _logger.LogWarning(ex, "Tentativa de criar empresa duplicada: {Nome}", request.Nome);
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao criar empresa.");
                return StatusCode(500, new { error = "Ocorreu um erro interno no servidor." });
            }
        }

        /// <summary>
        /// Lista todas as empresas do sistema. (Requer Admin)
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<EmpresaResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetEmpresas()
        {
            var empresas = await _empresaService.GetEmpresasAsync();
            return Ok(empresas);
        }

        /// <summary>
        /// Busca uma empresa pelo ID. (Requer Admin)
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(EmpresaResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetEmpresaById(Ulid id)
        {
            var empresa = await _empresaService.GetEmpresaByIdAsync(id);
            if (empresa == null)
            {
                return NotFound(new { error = $"Empresa com ID {id} não encontrada." });
            }
            return Ok(empresa);
        }

        /// <summary>
        /// Atualiza uma empresa existente. (Requer Admin)
        /// </summary>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(EmpresaResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateEmpresa(Ulid id, [FromBody] CreateEmpresaRequest request)
        {
            var empresa = await _empresaService.UpdateEmpresaAsync(id, request);
            if (empresa == null)
            {
                return NotFound(new { error = $"Empresa com ID {id} não encontrada." });
            }
            return Ok(empresa);
        }

        /// <summary>
        /// Exclui uma empresa. (Requer Admin)
        /// </summary>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteEmpresa(Ulid id)
        {
            var sucesso = await _empresaService.DeleteEmpresaAsync(id);
            if (!sucesso)
            {
                return NotFound(new { error = $"Empresa com ID {id} não encontrada." });
            }
            return NoContent();
        }
    }
}
