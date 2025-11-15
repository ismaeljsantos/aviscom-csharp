using Aviscom.DTOs.Usuario;
using NUlid;

namespace Aviscom.Services.Interfaces
{
    public interface IInstituicaoService
    {
        Task<InstituicaoResponse> CreateInstituicaoAsync(CreateInstituicaoRequest request);
        Task<IEnumerable<InstituicaoResponse>> GetInstituicoesAsync();
        Task<InstituicaoResponse?> GetInstituicaoByIdAsync(Ulid id);
        Task<InstituicaoResponse?> UpdateInstituicaoAsync(Ulid id, CreateInstituicaoRequest request);
        Task<bool> DeleteInstituicaoAsync(Ulid id);
    }
}
