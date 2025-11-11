using Aviscom.DTOs.Usuario;
using NUlid;

namespace Aviscom.Services.Interfaces
{
    public interface IUsuarioService
    {
        Task<UsuarioPessoaFisicaResponse> CreateUsuarioPessoaFisicaAsync(CreateUsuarioPessoaFisicaRequest request);
        Task<IEnumerable<UsuarioPessoaFisicaResponse>> GetUsuariosPessoaFisicaAsync();

        Task<UsuarioPessoaFisicaResponse?> GetPessoaFisicaByIdAsync(Ulid id);
    }
}
