using Aviscom.DTOs.Admin;
using Aviscom.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NUlid;

namespace Aviscom.Controllers
{
    [Route("api/admin/associacoes-funcao")]
    [ApiController]
    [Authorize(Policy = "Administrador")]
    public class UsuarioFuncaoController : ControllerBase
    {
        private readonly IUsuarioFuncaoService _usuarioFuncaoService;
        private readonly ILogger<UsuarioFuncaoController> _logger;

        public UsuarioFuncaoController(IUsuarioFuncaoService usuarioFuncaoService, ILogger<UsuarioFuncaoController> logger)
        {
            _usuarioFuncaoService = usuarioFuncaoService;
            _logger = logger;
        }

        // === MÉTODOS PESSOA FÍSICA (Existentes) ===

        /// <summary>
        /// Associa uma Função/Setor a um Usuário Pessoa Física. (Requer Admin)
        /// </summary>
        [HttpPost("pessoa-fisica")]
        [ProducesResponseType(typeof(UsuarioFuncaoResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AssignFuncaoToPf([FromBody] AssignFuncaoRequest request)
        {
            try
            {
                var novaAssociacao = await _usuarioFuncaoService.AssignFuncaoToPfAsync(request);
                return CreatedAtAction(
                    nameof(GetFuncoesByPfId),
                    new { usuarioPfId = novaAssociacao.FkUsuarioPfId },
                    novaAssociacao
                );
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex, "Falha ao associar função PF: {Message}", ex.Message);
                return NotFound(new { error = ex.Message });
            }
            catch (DbUpdateException ex)
            {
                _logger.LogWarning(ex, "Tentativa de associar função PF duplicada.");
                return BadRequest(new { error = "Esta associação (Usuário/Função/Setor) já existe." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro inesperado ao associar função a PF.");
                return StatusCode(500, new { error = "Ocorreu um erro interno no servidor." });
            }
        }

        /// <summary>
        /// Lista todas as associações de Funções/Setores de um Usuário Pessoa Física. (Requer Admin)
        /// </summary>
        [HttpGet("pessoa-fisica/{usuarioPfId}")]
        [ProducesResponseType(typeof(IEnumerable<UsuarioFuncaoResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetFuncoesByPfId(Ulid usuarioPfId)
        {
            var associacoes = await _usuarioFuncaoService.GetFuncoesByPfIdAsync(usuarioPfId);
            return Ok(associacoes);
        }

        /// <summary>
        /// Remove uma associação de Função/Setor de um Usuário Pessoa Física. (Requer Admin)
        /// </summary>
        [HttpDelete("pessoa-fisica")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> RemoveFuncaoFromPf([FromBody] AssignFuncaoRequest request)
        {
            try
            {
                var sucesso = await _usuarioFuncaoService.RemoveFuncaoFromPfAsync(request);
                if (!sucesso)
                {
                    return NotFound(new { error = "Associação (Usuário/Função/Setor) não encontrada." });
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao remover associação de função de PF.");
                return StatusCode(500, new { error = "Ocorreu um erro interno no servidor." });
            }
        }

        // ===================================
        // === NOVOS MÉTODOS PARA PESSOA JURÍDICA ===
        // ===================================

        /// <summary>
        /// Associa uma Função/Setor a um Usuário Pessoa Jurídica. (Requer Admin)
        /// </summary>
        [HttpPost("pessoa-juridica")]
        [ProducesResponseType(typeof(UsuarioFuncaoResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AssignFuncaoToPj([FromBody] AssignFuncaoPjRequest request)
        {
            try
            {
                var novaAssociacao = await _usuarioFuncaoService.AssignFuncaoToPjAsync(request);
                return CreatedAtAction(
                    nameof(GetFuncoesByPjId),
                    new { usuarioPjId = novaAssociacao.FkUsuarioPjId },
                    novaAssociacao
                );
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex, "Falha ao associar função PJ: {Message}", ex.Message);
                return NotFound(new { error = ex.Message });
            }
            catch (DbUpdateException ex)
            {
                _logger.LogWarning(ex, "Tentativa de associar função PJ duplicada.");
                return BadRequest(new { error = "Esta associação (Usuário/Função/Setor) já existe." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro inesperado ao associar função a PJ.");
                return StatusCode(500, new { error = "Ocorreu um erro interno no servidor." });
            }
        }

        /// <summary>
        /// Lista todas as associações de Funções/Setores de um Usuário Pessoa Jurídica. (Requer Admin)
        /// </summary>
        [HttpGet("pessoa-juridica/{usuarioPjId}")]
        [ProducesResponseType(typeof(IEnumerable<UsuarioFuncaoResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetFuncoesByPjId(Ulid usuarioPjId)
        {
            var associacoes = await _usuarioFuncaoService.GetFuncoesByPjIdAsync(usuarioPjId);
            return Ok(associacoes);
        }

        /// <summary>
        /// Remove uma associação de Função/Setor de um Usuário Pessoa Jurídica. (Requer Admin)
        /// </summary>
        [HttpDelete("pessoa-juridica")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> RemoveFuncaoFromPj([FromBody] AssignFuncaoPjRequest request)
        {
            try
            {
                var sucesso = await _usuarioFuncaoService.RemoveFuncaoFromPjAsync(request);
                if (!sucesso)
                {
                    return NotFound(new { error = "Associação (Usuário/Função/Setor) não encontrada." });
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao remover associação de função de PJ.");
                return StatusCode(500, new { error = "Ocorreu um erro interno no servidor." });
            }
        }
    }
}
