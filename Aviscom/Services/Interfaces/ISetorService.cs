using Aviscom.DTOs.Admin;
using NUlid;

namespace Aviscom.Services.Interfaces
{
    public interface ISetorService
    {
        Task<SetorResponse> CreateSetorAsync(CreateSetorRequest request);
        Task<IEnumerable<SetorResponse>> GetSetoresAsync();
        Task<SetorResponse?> GetSetorByIdAsync(Ulid id);
        Task<SetorResponse?> UpdateSetorAsync(Ulid id, CreateSetorRequest request);
        Task<bool> DeleteSetorAsync(Ulid id);
    }
}
