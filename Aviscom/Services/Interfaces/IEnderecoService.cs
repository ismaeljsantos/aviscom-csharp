using Aviscom.DTOs.Usuario;
using NUlid;

namespace Aviscom.Services.Interfaces
{
    public interface IEnderecoService
    {
        Task<EnderecoResponse> CreateEnderecoParaPessoaFisicaAsync(Ulid usuarioPfId, CreateEnderecoRequest request);
        Task<IEnumerable<EnderecoResponse>> GetEnderecosByPessoaFisicaIdAsync(Ulid usuarioPfId);
        Task<EnderecoResponse?> UpdateEnderecoAsync(Ulid id, CreateEnderecoRequest request);
        Task<bool> DeleteEnderecoAsync(Ulid id);
        Task<EnderecoResponse?> GetEnderecoByIdAsync(Ulid id);

        //=============================
        //====== MÉTODOS PARA PJ ======
        //=============================
        Task<EnderecoResponse> CreateEnderecoParaPessoaJuridicaAsync(Ulid usuarioPjId, CreateEnderecoRequest request);
        Task<IEnumerable<EnderecoResponse>> GetEnderecosByPessoaJuridicaIdAsync(Ulid usuarioPjId);
    }
}
