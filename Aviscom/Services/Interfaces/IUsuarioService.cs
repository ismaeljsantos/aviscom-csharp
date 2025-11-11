using Aviscom.DTOs.Usuario;

namespace Aviscom.Services.Interfaces
{
    public interface IUsuarioService
    {
        Task<UsuarioPessoaFisicaResponse> CreateUsuarioPessoaFisicaAsync(CreateUsuarioPessoaFisicaRequest request);
    }
}
