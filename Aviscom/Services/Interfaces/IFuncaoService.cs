using Aviscom.DTOs.Admin;
using NUlid;

namespace Aviscom.Services.Interfaces
{
    public interface IFuncaoService
    {
        Task<FuncaoResponse> CreateFuncaoAsync(CreateFuncaoRequest request);
        Task<IEnumerable<FuncaoResponse>> GetFuncoesAsync();
        Task<FuncaoResponse?> GetFuncaoByIdAsync(Ulid id);
        Task<FuncaoResponse?> UpdateFuncaoAsync(Ulid id, CreateFuncaoRequest request);
        Task<bool> DeleteFuncaoAsync(Ulid id);
    }
}