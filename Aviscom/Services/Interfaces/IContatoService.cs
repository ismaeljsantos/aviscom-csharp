using Aviscom.DTOs.Usuario;
using NUlid;

namespace Aviscom.Services.Interfaces
{
    public interface IContatoService
    {
        // === Métodos aninhados (associados ao usuário) ===
        Task<ContatoResponse> CreateContatoParaPessoaFisicaAsync(Ulid usuarioPfId, CreateContatoRequest request);
        Task<IEnumerable<ContatoResponse>> GetContatosByPessoaFisicaIdAsync(Ulid usuarioPfId);

        // === Métodos diretos (pelo ID do contato) ===
        Task<ContatoResponse?> GetContatoByIdAsync(Ulid id);
        Task<ContatoResponse?> UpdateContatoAsync(Ulid id, CreateContatoRequest request);
        Task<bool> DeleteContatoAsync(Ulid id);
    }
}