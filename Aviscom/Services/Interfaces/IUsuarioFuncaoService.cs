using Aviscom.DTOs.Admin;
using NUlid;

namespace Aviscom.Services.Interfaces
{
    public interface IUsuarioFuncaoService
    {
        Task<UsuarioFuncaoResponse> AssignFuncaoToPfAsync(AssignFuncaoRequest request);
        Task<bool> RemoveFuncaoFromPfAsync(AssignFuncaoRequest request);
        Task<IEnumerable<UsuarioFuncaoResponse>> GetFuncoesByPfIdAsync(Ulid usuarioPfId);


        // === NOVOS MÉTODOS PARA PJ ===
        Task<UsuarioFuncaoResponse> AssignFuncaoToPjAsync(AssignFuncaoPjRequest request);
        Task<bool> RemoveFuncaoFromPjAsync(AssignFuncaoPjRequest request);
        Task<IEnumerable<UsuarioFuncaoResponse>> GetFuncoesByPjIdAsync(Ulid usuarioPjId);
        Task<IEnumerable<FuncaoAssociacaoResponse>> GetUsuariosByFuncaoTituloAsync(string tituloFuncao);
    }
}
