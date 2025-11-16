using Aviscom.DTOs.Usuario;
using Aviscom.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NUlid;

namespace Aviscom.Controllers
{
 
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioService _usuarioService;
        private readonly ILogger<UsuarioController> _logger;

        public UsuarioController(IUsuarioService usuarioService,
                                 ILogger<UsuarioController> logger)
        {
            _usuarioService = usuarioService;
            _logger = logger;
        }

        ///<summary>
        ///Cria um novo usuário do tipo Pessoa Física
        ///</summary>
        [HttpPost("pessoa-fisica")]
        [ProducesResponseType(typeof(UsuarioPessoaFisicaResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]

        public async Task<IActionResult> CreatePessoaFisica([FromBody] CreateUsuarioPessoaFisicaRequest request)
        {
            try
            {
                var novoUsuario = await _usuarioService.CreateUsuarioPessoaFisicaAsync(request);

                return CreatedAtAction(
                    nameof(GetPessoaFisicaById),
                    new { id = novoUsuario.Id },
                    novoUsuario
                    );
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Falha na criação do usuário PF (regra de negócio): {Message}", ex.Message);
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro inesperado ao criar usuario PF");
                return StatusCode(500, new { error = "Ocorreu um erro interno no servidor." });
            }
        }

        /// <summary>
        /// Busca uma lista de todos os usuários Pessoa Física.
        /// </summary>
        [HttpGet("pessoa-fisica")] // GET /api/usuarios/pessoa-fisica
        [ProducesResponseType(typeof(IEnumerable<UsuarioPessoaFisicaResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetUsuariosPessoaFisica()
        {
            try
            {
                var usuarios = await _usuarioService.GetUsuariosPessoaFisicaAsync();
                return Ok(usuarios);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro inesperado ao buscar lista de usuários PF.");
                return StatusCode(500, new { error = "Ocorreu um erro interno no servidor." });
            }
        }


        /// <summary>
        /// Busca um usuário Pessoa Física pelo ID.
        /// </summary>
        [HttpGet("pessoa-fisica/{id}")]
        [ProducesResponseType(typeof(UsuarioPessoaFisicaResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetPessoaFisicaById(Ulid id)
        {
            try
            {
                var usuario = await _usuarioService.GetPessoaFisicaByIdAsync(id);

                if (usuario == null)
                {
                    return NotFound(new { error = $"Usuário com ID {id} não encontrado." });
                }

                return Ok(usuario);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro inesperado ao buscar usuário PF pelo ID {UserId}.", id);
                return StatusCode(500, new { error = "Ocorreu um erro interno no servidor." });
            }
        }


        /// <summary>
        /// Atualiza parcialmente um usuário Pessoa Física.
        /// </summary>
        [HttpPatch("pessoa-fisica/{id}")]
        [ProducesResponseType(typeof(UsuarioPessoaFisicaResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdatePessoaFisica(Ulid id, [FromBody] UpdateUsuarioPessoaFisicaRequest request)
        {
            try
            {
                var usuario = await _usuarioService.UpdatePessoaFisicaAsync(id, request);

                if (usuario == null)
                {
                    return NotFound(new { error = $"Usuário com ID {id} não encontrado." });
                }

                return Ok(usuario);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro inesperado ao ATUALIZAR usuário PF pelo ID {UserId}.", id);
                return StatusCode(500, new { error = "Ocorreu um erro interno no servidor." });
            }
        }

        /// <summary>
        /// Exclui um usuário Pessoa Física.
        /// </summary>
        [HttpDelete("pessoa-fisica/{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeletePessoaFisica(Ulid id)
        {
            try
            {
                var foiExcluido = await _usuarioService.DeletePessoaFisicaAsync(id);

                if (!foiExcluido)
                {
                    return NotFound(new { error = $"Usuário com ID {id} não encontrado." });
                }

                // 204 No Content é a resposta padrão para um DELETE bem-sucedido
                // que não precisa retornar um corpo.
                return NoContent();
            }
            catch (Exception ex)
            {
                // ATENÇÃO: Se o usuário tiver dados relacionados (ex: Endereços)
                // e a regra de OnDelete não for 'Cascade', isso pode falhar
                // com uma DbUpdateException (conflito de chave estrangeira).
                _logger.LogError(ex, "Erro inesperado ao EXCLUIR usuário PF pelo ID {UserId}.", id);
                return StatusCode(500, new { error = "Ocorreu um erro interno no servidor." });
            }
        }


        // ===================================
        // ====== ENDPOINTS (CRUD PJ) ========
        // ===================================


        /// <summary>
        /// Cria um novo utilizador do tipo Pessoa Jurídica.
        /// </summary>
        [HttpPost("pessoa-juridica")]
        [AllowAnonymous] // Aberto para registo público
        [ProducesResponseType(typeof(UsuarioPessoaJuridicaResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> CreatePessoaJuridica([FromBody] CreateUsuarioPessoaJuridicaRequest request)
        {
            try
            {
                var novoUsuarioPJ = await _usuarioService.CreateUsuarioPessoaJuridicaAsync(request);

                // Retorna 201 Created
                return CreatedAtAction(
                    nameof(GetPessoaJuridicaById), // Nome do método 'Get'
                    new { id = novoUsuarioPJ.Id },  // Parâmetro da rota do 'Get'
                    novoUsuarioPJ);                 // O objeto criado
            }
            catch (KeyNotFoundException ex) // Se o Responsável PF não for encontrado
            {
                _logger.LogWarning("Falha ao criar PJ: {Message}", ex.Message);
                return NotFound(new { error = ex.Message });
            }
            catch (InvalidOperationException ex) // Se o CNPJ já existir
            {
                _logger.LogWarning("Falha ao criar PJ: {Message}", ex.Message);
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro inesperado ao criar utilizador PJ.");
                return StatusCode(500, new { error = "Ocorreu um erro interno no servidor." });
            }
        }

        /// <summary>
        /// Busca uma lista de todos os utilizadores Pessoa Jurídica. (Requer Admin)
        /// </summary>
        [HttpGet("pessoa-juridica")]
        [Authorize(Policy = "Administrador")] // 1. APENAS ADMIN
        [ProducesResponseType(typeof(IEnumerable<UsuarioPessoaJuridicaResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetUsuariosPessoaJuridica()
        {
            var usuariosPJ = await _usuarioService.GetUsuariosPessoaJuridicaAsync();
            return Ok(usuariosPJ);
        }

        /// <summary>
        /// Busca um utilizador Pessoa Jurídica pelo ID. (Requer Login)
        /// </summary>
        [HttpGet("pessoa-juridica/{id}")]
        [ProducesResponseType(typeof(UsuarioPessoaJuridicaResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetPessoaJuridicaById(Ulid id)
        {
            // (Usa o [Authorize] da classe - requer login)
            var usuarioPJ = await _usuarioService.GetPessoaJuridicaByIdAsync(id);
            if (usuarioPJ == null)
            {
                return NotFound(new { error = $"Utilizador Pessoa Jurídica com ID {id} não encontrado." });
            }
            return Ok(usuarioPJ);
        }

        /// <summary>
        /// Atualiza parcialmente um utilizador Pessoa Jurídica. (Requer Login)
        /// </summary>
        [HttpPatch("pessoa-juridica/{id}")]
        [ProducesResponseType(typeof(UsuarioPessoaJuridicaResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdatePessoaJuridica(Ulid id, [FromBody] UpdateUsuarioPessoaJuridicaRequest request)
        {
            // (Usa o [Authorize] da classe - requer login)
            // TODO FUTURO: Adicionar segurança (só o próprio ou Admin pode atualizar)
            try
            {
                var usuarioPJ = await _usuarioService.UpdatePessoaJuridicaAsync(id, request);
                if (usuarioPJ == null)
                {
                    return NotFound(new { error = $"Utilizador Pessoa Jurídica com ID {id} não encontrado." });
                }
                return Ok(usuarioPJ);
            }
            catch (KeyNotFoundException ex) // Se o novo Responsável não for encontrado
            {
                _logger.LogWarning("Falha ao atualizar PJ: {Message}", ex.Message);
                return NotFound(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao atualizar Utilizador PJ {UserId}", id);
                return StatusCode(500, new { error = "Ocorreu um erro interno no servidor." });
            }
        }

        /// <summary>
        /// Exclui um utilizador Pessoa Jurídica. (Requer Admin)
        /// </summary>
        [HttpDelete("pessoa-juridica/{id}")]
        [Authorize(Policy = "Administrador")] // 2. APENAS ADMIN
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeletePessoaJuridica(Ulid id)
        {
            var sucesso = await _usuarioService.DeletePessoaJuridicaAsync(id);
            if (!sucesso)
            {
                return NotFound(new { error = $"Utilizador Pessoa Jurídica com ID {id} não encontrado." });
            }
            return NoContent();
        }
    }
}
