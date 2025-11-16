using Aviscom.Dtos.Auth;
using Aviscom.DTOs.Auth;

namespace Aviscom.Services.Interfaces
{
    public interface IAuthService
    {
        Task<LoginResponse?> LoginPessoaFisicaAsync(LoginRequest request);
        Task<LoginResponse?> LoginPessoaJuridicaAsync(LoginPjRequest request);
    }
}