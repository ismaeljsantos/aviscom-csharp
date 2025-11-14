using Aviscom.Dtos.Auth;

namespace Aviscom.Services.Interfaces
{
    public interface IAuthService
    {
        Task<LoginResponse?> LoginPessoaFisicaAsync(LoginRequest request);
    }
}