using Aviscom.DTOs.Usuario;
using NUlid;

namespace Aviscom.Services.Interfaces
{
    public interface IExperienciaProfissionalService
    {
        // === Métodos aninhados (associados ao utilizador) ===
        Task<ExperienciaProfissionalResponse> CreateExperienciaParaPfAsync(Ulid usuarioPfId, CreateExperienciaProfissionalRequest request);
        Task<IEnumerable<ExperienciaProfissionalResponse>> GetExperienciasByPfIdAsync(Ulid usuarioPfId);

        // === Métodos diretos (pelo ID da experiência) ===
        Task<ExperienciaProfissionalResponse?> GetExperienciaByIdAsync(Ulid id);
        Task<ExperienciaProfissionalResponse?> UpdateExperienciaAsync(Ulid id, CreateExperienciaProfissionalRequest request);
        Task<bool> DeleteExperienciaAsync(Ulid id);
    }
}
