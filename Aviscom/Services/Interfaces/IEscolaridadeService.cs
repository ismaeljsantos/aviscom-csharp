using Aviscom.DTOs.Usuario;
using NUlid;

namespace Aviscom.Services.Interfaces
{
    public interface IEscolaridadeService
    {
        // === Métodos aninhados (associados ao utilizador) ===
        Task<EscolaridadeResponse> CreateEscolaridadeParaPfAsync(Ulid usuarioPfId, CreateEscolaridadeRequest request);
        Task<IEnumerable<EscolaridadeResponse>> GetEscolaridadesByPfIdAsync(Ulid usuarioPfId);

        // === Métodos diretos (pelo ID da escolaridade) ===
        Task<EscolaridadeResponse?> GetEscolaridadeByIdAsync(Ulid id);
        Task<EscolaridadeResponse?> UpdateEscolaridadeAsync(Ulid id, CreateEscolaridadeRequest request);
        Task<bool> DeleteEscolaridadeAsync(Ulid id);
    }
}
