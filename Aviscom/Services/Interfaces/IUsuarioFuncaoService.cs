using Aviscom.DTOs.Admin;
using NUlid;

namespace Aviscom.Services.Interfaces
{
    public interface IUsuarioFuncaoService
    {
        Task<UsuarioFuncaoResponse> AssignFuncaoToPfAsync(AssignFuncaoRequest request);
        Task<bool> RemoveFuncaoFromPfAsync(AssignFuncaoRequest request);
        Task<IEnumerable<UsuarioFuncaoResponse>> GetFuncoesByPfIdAsync(Ulid usuarioPfId);
    }
}
