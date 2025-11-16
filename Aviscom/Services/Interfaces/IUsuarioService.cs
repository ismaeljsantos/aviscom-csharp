using Aviscom.DTOs.Usuario;
using NUlid;

namespace Aviscom.Services.Interfaces
{
    public interface IUsuarioService
    {
        Task<UsuarioPessoaFisicaResponse> CreateUsuarioPessoaFisicaAsync(CreateUsuarioPessoaFisicaRequest request);
        Task<IEnumerable<UsuarioPessoaFisicaResponse>> GetUsuariosPessoaFisicaAsync();

        Task<UsuarioPessoaFisicaResponse?> GetPessoaFisicaByIdAsync(Ulid id);
        Task<UsuarioPessoaFisicaResponse?> UpdatePessoaFisicaAsync(Ulid id, UpdateUsuarioPessoaFisicaRequest request);
        Task<bool> DeletePessoaFisicaAsync(Ulid id);

        // Métodos PJ
        Task<UsuarioPessoaJuridicaResponse> CreateUsuarioPessoaJuridicaAsync(CreateUsuarioPessoaJuridicaRequest request);
        Task<IEnumerable<UsuarioPessoaJuridicaResponse>> GetUsuariosPessoaJuridicaAsync();
        Task<UsuarioPessoaJuridicaResponse?> GetPessoaJuridicaByIdAsync(Ulid id);
        Task<UsuarioPessoaJuridicaResponse?> UpdatePessoaJuridicaAsync(Ulid id, UpdateUsuarioPessoaJuridicaRequest request);
        Task<bool> DeletePessoaJuridicaAsync(Ulid id);
    }
}
