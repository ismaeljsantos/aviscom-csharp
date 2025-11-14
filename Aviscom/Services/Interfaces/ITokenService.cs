using Aviscom.Models.Usuario;

namespace Aviscom.Services.Interfaces
{
    public interface ITokenService
    {
        string GenerateToken(UsuarioPessoaFisica usuario, List<string> funcoes);
    }
}