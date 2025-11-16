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
    public class ContatoController : ControllerBase
    {
        private readonly IContatoService _contatoService;
        private readonly ILogger<ContatoController> _logger;

        public ContatoController(IContatoService contatoService, ILogger<ContatoController> logger)
        {
            _contatoService = contatoService;
            _logger = logger;
        }

        /// <summary>
        /// Cria um novo contato para um usuário Pessoa Física.
        /// </summary>
        [HttpPost("usuarios/pessoa-fisica/{usuarioPfId}/contatos")]
        [ProducesResponseType(typeof(ContatoResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> CreateContatoParaPessoaFisica([FromRoute] Ulid usuarioPfId, [FromBody] CreateContatoRequest request)
        {
            try
            {
                var novoContato = await _contatoService.CreateContatoParaPessoaFisicaAsync(usuarioPfId, request);

                return CreatedAtAction(
                    nameof(GetContatoById),      // Nome do método 'Get'
                    new { id = novoContato.Id }, // Parâmetro da rota do 'Get'
                    novoContato);                // O objeto criado
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning("Falha ao criar contato: {Message}", ex.Message);
                return NotFound(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro inesperado ao criar contato para o usuário {UsuarioId}", usuarioPfId);
                return StatusCode(500, new { error = "Ocorreu um erro interno no servidor." });
            }
        }


        /// <summary>
        /// Lista todos os contatos de um usuário Pessoa Física.
        /// </summary>
        [HttpGet("usuarios/pessoa-fisica/{usuarioPfId}/contatos")]
        [ProducesResponseType(typeof(IEnumerable<ContatoResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetContatosPessoaFisica([FromRoute] Ulid usuarioPfId)
        {
            try
            {
                var contatos = await _contatoService.GetContatosByPessoaFisicaIdAsync(usuarioPfId);
                return Ok(contatos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro inesperado ao buscar contatos do usuário {UsuarioId}", usuarioPfId);
                return StatusCode(500, new { error = "Ocorreu um erro interno no servidor." });
            }
        }


        /// <summary>
        /// Busca um contato pelo seu ID.
        /// </summary>
        [HttpGet("contatos/{id}")]
        [ProducesResponseType(typeof(ContatoResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetContatoById(Ulid id)
        {
            try
            {
                var contato = await _contatoService.GetContatoByIdAsync(id);
                if (contato == null)
                {
                    return NotFound(new { error = $"Contato com ID {id} não encontrado." });
                }
                return Ok(contato);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar Contato {ContatoId}", id);
                return StatusCode(500, new { error = "Ocorreu um erro interno no servidor." });
            }
        }

        /// <summary>
        /// Atualiza um contato existente pelo seu ID.
        /// </summary>
        [HttpPut("contatos/{id}")]
        [ProducesResponseType(typeof(ContatoResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateContato(Ulid id, [FromBody] CreateContatoRequest request)
        {
            try
            {
                var contato = await _contatoService.UpdateContatoAsync(id, request);
                if (contato == null)
                {
                    return NotFound(new { error = $"Contato com ID {id} não encontrado." });
                }
                return Ok(contato);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao atualizar Contato {ContatoId}", id);
                return StatusCode(500, new { error = "Ocorreu um erro interno no servidor." });
            }
        }

        /// <summary>
        /// Exclui um contato pelo seu ID.
        /// </summary>
        [HttpDelete("contatos/{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteContato(Ulid id)
        {
            try
            {
                var sucesso = await _contatoService.DeleteContatoAsync(id);
                if (!sucesso)
                {
                    return NotFound(new { error = $"Contato com ID {id} não encontrado." });
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao excluir Contato {ContatoId}", id);
                return StatusCode(500, new { error = "Ocorreu um erro interno no servidor." });
            }
        }

        // === NOVOS ENDPOINTS DE PESSOA JURÍDICA ===

        /// <summary>
        /// Cria um novo contacto para um utilizador Pessoa Jurídica. (Requer Login)
        /// </summary>
        [HttpPost("usuarios/pessoa-juridica/{usuarioPjId}/contatos")]
        [ProducesResponseType(typeof(ContatoResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> CreateContatoParaPessoaJuridica([FromRoute] Ulid usuarioPjId, [FromBody] CreateContatoRequest request)
        {
            try
            {
                var novoContato = await _contatoService.CreateContatoParaPessoaJuridicaAsync(usuarioPjId, request);
                return CreatedAtAction(nameof(GetContatoById), new { id = novoContato.Id }, novoContato);
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning("Falha ao criar contacto PJ: {Message}", ex.Message);
                return NotFound(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro inesperado ao criar contacto para o utilizador PJ {UsuarioId}", usuarioPjId);
                return StatusCode(500, new { error = "Ocorreu um erro interno no servidor." });
            }
        }

        /// <summary>
        /// Lista todos os contactos de um utilizador Pessoa Jurídica. (Requer Login)
        /// </summary>
        [HttpGet("usuarios/pessoa-juridica/{usuarioPjId}/contatos")]
        [ProducesResponseType(typeof(IEnumerable<ContatoResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetContatosPessoaJuridica([FromRoute] Ulid usuarioPjId)
        {
            var contatos = await _contatoService.GetContatosByPessoaJuridicaIdAsync(usuarioPjId);
            return Ok(contatos);
        }
    }
}
