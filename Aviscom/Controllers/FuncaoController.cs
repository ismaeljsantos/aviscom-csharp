using Aviscom.DTOs.Admin;
using Aviscom.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NUlid;

namespace Aviscom.Controllers
{
    [ApiController]
    [Route("api/admin/funcoes")]
    [Authorize(Policy = "Administrador")] 
    public class FuncaoController : ControllerBase
    {
        private readonly IFuncaoService _funcaoService;
        private readonly ILogger<FuncaoController> _logger;

        public FuncaoController(IFuncaoService funcaoService, ILogger<FuncaoController> logger)
        {
            _funcaoService = funcaoService;
            _logger = logger;
        }

        /// <summary>
        /// Cria uma nova função (role). (Requer Admin)
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(FuncaoResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateFuncao([FromBody] CreateFuncaoRequest request)
        {
            try
            {
                var novaFuncao = await _funcaoService.CreateFuncaoAsync(request);
                return CreatedAtAction(nameof(GetFuncaoById), new { id = novaFuncao.Id }, novaFuncao);
            }
            catch (InvalidOperationException ex) // Erro de "já existe"
            {
                _logger.LogWarning(ex, "Tentativa de criar função duplicada: {Titulo}", request.Titulo);
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao criar função.");
                return StatusCode(500, new { error = "Ocorreu um erro interno no servidor." });
            }
        }

        /// <summary>
        /// Lista todas as funções (roles) do sistema. (Requer Admin)
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<FuncaoResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetFuncoes()
        {
            var funcoes = await _funcaoService.GetFuncoesAsync();
            return Ok(funcoes);
        }

        /// <summary>
        /// Busca uma função (role) pelo ID. (Requer Admin)
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(FuncaoResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetFuncaoById(Ulid id)
        {
            var funcao = await _funcaoService.GetFuncaoByIdAsync(id);
            if (funcao == null)
            {
                return NotFound(new { error = $"Função com ID {id} não encontrada." });
            }
            return Ok(funcao);
        }

        /// <summary>
        /// Atualiza uma função (role) existente. (Requer Admin)
        /// </summary>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(FuncaoResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateFuncao(Ulid id, [FromBody] CreateFuncaoRequest request)
        {
            var funcao = await _funcaoService.UpdateFuncaoAsync(id, request);
            if (funcao == null)
            {
                return NotFound(new { error = $"Função com ID {id} não encontrada." });
            }
            return Ok(funcao);
        }

        /// <summary>
        /// Exclui uma função (role). (Requer Admin)
        /// </summary>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteFuncao(Ulid id)
        {
            var sucesso = await _funcaoService.DeleteFuncaoAsync(id);
            if (!sucesso)
            {
                return NotFound(new { error = $"Função com ID {id} não encontrada." });
            }
            return NoContent();
        }
    }
}