using Aviscom.Data;
using Aviscom.DTOs.Usuario;
using Aviscom.Models.Usuario;
using Aviscom.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using NUlid;

namespace Aviscom.Services
{
    public class EnderecaoService : IEnderecoService
    {
        private readonly AviscomContext _context;
        private readonly ILogger _logger;

        public EnderecaoService(AviscomContext context, ILogger<IEnderecoService> logger)
        {
            _context = context;
            _logger = logger;
        }
        public async Task<EnderecoResponse> CreateEnderecoParaPessoaFisicaAsync(Ulid usuarioPfId, CreateEnderecoRequest request)
        {
            var usuario = await _context.UsuariosFisicos.FindAsync(usuarioPfId);
            if (usuario == null) 
            {
                throw  new KeyNotFoundException($"Usuário Pessoa Fisica com ID {usuarioPfId} não encontrado.");

            }

            var novoEndereco = new Endereco
            {
                TipoLogradouro = request.TipoLogradouro,
                Logradouro = request.TipoLogradouro,
                Numero = request.Numero,
                Complemento = request.Complemento,
                Bairro = request.Bairro,
                Cidade = request.Cidade,
                Estado = request.Estado,
                Cep = request.Cidade,
                FkPessoaFisicaId = usuarioPfId
            };

            await _context.Enderecos.AddAsync(novoEndereco);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Endereco {EnderecoId} criado para o usuário {UsuarioId}", novoEndereco.Id, usuarioPfId);

            return MapearParaResponse(novoEndereco);
           
        }

        public async Task<IEnumerable<EnderecoResponse>> GetEnderecosByPessoaFisicaIdAsync(Ulid usuarioPfId)
        {
            var enderecos = await _context.Enderecos
                .AsNoTracking()
                .Where(e => e.FkPessoaFisicaId == usuarioPfId)
                .Select(e => new EnderecoResponse
                {
                    Id = e.Id,
                    TipoLogradouro = e.TipoLogradouro,
                    Logradouro = e.TipoLogradouro,
                    Numero = e.Numero,
                    Complemento = e.Complemento,
                    Bairro = e.Bairro,
                    Cidade = e.Cidade,
                    Estado = e.Estado,
                    Cep = e.Cep,
                    FkPessoaFisicaId = usuarioPfId,
                    FkPessoaJuridicaId = usuarioPfId
                }) 
                .ToListAsync();

            return enderecos;
        }

        public async  Task<EnderecoResponse?> UpdateEnderecoAsync(Ulid id, CreateEnderecoRequest request)
        {
            var endereco = await _context.Enderecos.FindAsync(id);
            if (endereco == null)
            {
                return null;
            }

            endereco.TipoLogradouro = request.TipoLogradouro;
            endereco.Logradouro = request.Logradouro;
            endereco.Numero =   request.Numero;
            endereco.Complemento = request.Complemento;
            endereco.Bairro = request.Bairro;
            endereco.Cidade = request.Cidade;
            endereco.Estado = request.Estado;
            endereco.Cep = request.Cep;

            await _context.SaveChangesAsync();
            _logger.LogInformation("Endereço {EnderecoId} atualizado.", id);
            return MapearParaResponse(endereco);
        }

        public async Task<bool> DeleteEnderecoAsync(Ulid id)
        {
            var endereco = await _context.Enderecos.FindAsync(id);
            if (endereco == null)
            {
                return false; 
            }

            _context.Enderecos.Remove(endereco);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Endereço {EnderecoId} excluído.", id);
            return true;
        }

        public async Task<EnderecoResponse?> GetEnderecoByIdAsync(Ulid id)
        {
            var endereco = await _context.Enderecos
                .AsNoTracking()
                .Where(e => e.Id == id)
                .Select(e => new EnderecoResponse
                {
                    Id = e.Id,
                    TipoLogradouro = e.TipoLogradouro,
                    Logradouro = e.Logradouro,
                    Numero = e.Numero,
                    Complemento = e.Complemento,
                    Bairro = e.Bairro,
                    Cidade = e.Cidade,
                    Estado = e.Estado,
                    Cep = e.Cep,
                    FkPessoaFisicaId = e.FkPessoaFisicaId,
                    FkPessoaJuridicaId = e.FkPessoaJuridicaId
                })
                .FirstOrDefaultAsync();

            return endereco;
        }

        private EnderecoResponse MapearParaResponse(Endereco endereco)
        {
            return new EnderecoResponse
            {
                Id = endereco.Id,
                TipoLogradouro = endereco.TipoLogradouro,
                Logradouro = endereco.Logradouro,
                Numero = endereco.Numero,
                Complemento = endereco.Complemento,
                Bairro = endereco.Bairro,
                Cidade = endereco.Cidade,
                Estado = endereco.Estado,
                Cep = endereco.Cep,
                FkPessoaFisicaId = endereco.FkPessoaFisicaId,
                FkPessoaJuridicaId = endereco.FkPessoaJuridicaId
            };
        }

        //=============================
        //====== MÉTODOS PARA PJ ======
        //=============================

        public async Task<EnderecoResponse> CreateEnderecoParaPessoaJuridicaAsync(Ulid usuarioPjId, CreateEnderecoRequest request)
        {
            // 1. Verifica se o utilizador "dono" (PJ) existe
            var usuario = await _context.UsuariosJuridicos.FindAsync(usuarioPjId);
            if (usuario == null)
            {
                throw new KeyNotFoundException($"Utilizador Pessoa Jurídica com ID {usuarioPjId} não encontrado.");
            }

            // 2. Mapeia o DTO para a Entidade
            var novoEndereco = new Endereco
            {
                TipoLogradouro = request.TipoLogradouro,
                Logradouro = request.Logradouro,
                Numero = request.Numero,
                Complemento = request.Complemento,
                Bairro = request.Bairro,
                Cidade = request.Cidade,
                Estado = request.Estado,
                Cep = request.Cep,
                FkPessoaJuridicaId = usuarioPjId // ASSOCIA O ENDEREÇO À PJ
            };

            // 3. Salva no banco
            await _context.Enderecos.AddAsync(novoEndereco);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Endereço {EnderecoId} criado para o utilizador PJ {UsuarioId}", novoEndereco.Id, usuarioPjId);

            // 4. Retorna o DTO de Resposta
            return MapearParaResponse(novoEndereco);
        }

        public async Task<IEnumerable<EnderecoResponse>> GetEnderecosByPessoaJuridicaIdAsync(Ulid usuarioPjId)
        {
            return await _context.Enderecos
                .AsNoTracking()
                .Where(e => e.FkPessoaJuridicaId == usuarioPjId) // Filtra por PJ ID
                .Select(e => new EnderecoResponse
                {
                    Id = e.Id,
                    TipoLogradouro = e.TipoLogradouro,
                    Logradouro = e.Logradouro,
                    Numero = e.Numero,
                    Complemento = e.Complemento,
                    Bairro = e.Bairro,
                    Cidade = e.Cidade,
                    Estado = e.Estado,
                    Cep = e.Cep,
                    FkPessoaFisicaId = e.FkPessoaFisicaId,
                    FkPessoaJuridicaId = e.FkPessoaJuridicaId
                })
                .ToListAsync();
        }

    }
}
