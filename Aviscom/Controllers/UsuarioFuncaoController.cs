using Aviscom.DTOs.Admin;
using Aviscom.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NUlid;

namespace Aviscom.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioFuncaoController : ControllerBase
    {
        private readonly IUsuarioFuncaoService _usuarioFuncaoService;
        private readonly ILogger<UsuarioFuncaoController> _logger;

        public UsuarioFuncaoController(IUsuarioFuncaoService usuarioFuncaoService, ILogger<UsuarioFuncaoController> logger)
        {
            _usuarioFuncaoService = usuarioFuncaoService;
            _logger = logger;
        }

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
                // Retorna 201 Created com a nova associação
                return CreatedAtAction(
                    nameof(GetFuncoesByPfId),
                    new { usuarioPfId = novaAssociacao.FkUsuarioPfId },
                    novaAssociacao
                );
            }
            catch (KeyNotFoundException ex) // Se o Usuário, Função ou Setor não for encontrado
            {
                _logger.LogWarning(ex, "Falha ao associar função: {Message}", ex.Message);
                return NotFound(new { error = ex.Message });
            }
            catch (DbUpdateException ex) // Se a associação já existir
            {
                _logger.LogWarning(ex, "Tentativa de associar função duplicada.");
                return BadRequest(new { error = "Esta associação (Usuário/Função/Setor) já existe." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro inesperado ao associar função.");
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
            // Usamos o DTO 'AssignFuncaoRequest' no body para passar a chave composta
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
                _logger.LogError(ex, "Erro ao remover associação de função.");
                return StatusCode(500, new { error = "Ocorreu um erro interno no servidor." });
            }
        }
    }
}
