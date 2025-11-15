using Aviscom.DTOs.Usuario;
using NUlid;

namespace Aviscom.Services.Interfaces
{
    public interface IEmpresaService
    {
        Task<EmpresaResponse> CreateEmpresaAsync(CreateEmpresaRequest request);
        Task<IEnumerable<EmpresaResponse>> GetEmpresasAsync();
        Task<EmpresaResponse?> GetEmpresaByIdAsync(Ulid id);
        Task<EmpresaResponse?> UpdateEmpresaAsync(Ulid id, CreateEmpresaRequest request);
        Task<bool> DeleteEmpresaAsync(Ulid id);
    }
}
