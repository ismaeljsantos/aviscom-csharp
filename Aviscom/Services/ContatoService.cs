using Aviscom.Data;
using Aviscom.DTOs.Usuario;
using Aviscom.Models.Usuario;
using Aviscom.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using NUlid;
using System.Text.RegularExpressions;

namespace Aviscom.Services
{
    public class ContatoService : IContatoService
    {
        private readonly AviscomContext _context;
        private readonly ILogger<ContatoService> _logger;

        public ContatoService(AviscomContext context, ILogger<ContatoService> logger)
        {
            _context = context;
            _logger = logger;
        }

   

        public async Task<ContatoResponse> CreateContatoParaPessoaFisicaAsync(Ulid usuarioPfId, CreateContatoRequest request)
        {
            
            var usuario = await _context.UsuariosFisicos.FindAsync(usuarioPfId);
            if (usuario == null)
            {
                throw new KeyNotFoundException($"Usuário Pessoa Física com ID {usuarioPfId} não encontrado.");
            }

            string valorParaSalvar = request.Valor;
            if (request.Tipo.Equals("Telefone", StringComparison.OrdinalIgnoreCase) ||
                request.Tipo.Equals("Celular", StringComparison.OrdinalIgnoreCase))
            {
                valorParaSalvar = LimparTelefone(request.Valor);
            }

            var novoContato = new Contato
            {
                Tipo = request.Tipo,
                Valor = request.Valor,
                FkPessoaFisicaId = usuarioPfId 
            };

            
            await _context.Contatos.AddAsync(novoContato);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Contato {ContatoId} criado para o usuário {UsuarioId}", novoContato.Id, usuarioPfId);

           
            return MapearParaResponse(novoContato);
        }

        public async Task<IEnumerable<ContatoResponse>> GetContatosByPessoaFisicaIdAsync(Ulid usuarioPfId)
        {
            return await _context.Contatos
                .AsNoTracking()
                .Where(c => c.FkPessoaFisicaId == usuarioPfId)
                .Select(c => new ContatoResponse
                {
                    Id = c.Id,
                    Tipo = c.Tipo,
                    Valor = c.Valor,
                    FkPessoaFisicaId = c.FkPessoaFisicaId,
                    FkPessoaJuridicaId = c.FkPessoaJuridicaId
                })
                .ToListAsync();
        }


        public async Task<ContatoResponse?> GetContatoByIdAsync(Ulid id)
        {
            return await _context.Contatos
                .AsNoTracking()
                .Where(c => c.Id == id)
                .Select(c => new ContatoResponse
                {
                    Id = c.Id,
                    Tipo = c.Tipo,
                    Valor = c.Valor,
                    FkPessoaFisicaId = c.FkPessoaFisicaId,
                    FkPessoaJuridicaId = c.FkPessoaJuridicaId
                })
                .FirstOrDefaultAsync();
        }

        public async Task<ContatoResponse?> UpdateContatoAsync(Ulid id, CreateContatoRequest request)
        {
            var contato = await _context.Contatos.FindAsync(id);
            if (contato == null)
            {
                return null; 
            }

            string valorParaSalvar = request.Valor;
            if (request.Tipo.Equals("Telefone", StringComparison.OrdinalIgnoreCase) ||
                request.Tipo.Equals("Celular", StringComparison.OrdinalIgnoreCase))
            {
                valorParaSalvar = LimparTelefone(request.Valor);
            }

            // Mapeamento para atualização (PUT/PATCH)
            contato.Tipo = request.Tipo;
            contato.Valor = request.Valor;

            await _context.SaveChangesAsync();
            _logger.LogInformation("Contato {ContatoId} atualizado.", id);
            return MapearParaResponse(contato);
        }

        public async Task<bool> DeleteContatoAsync(Ulid id)
        {
            var contato = await _context.Contatos.FindAsync(id);
            if (contato == null)
            {
                return false; 
            }

            _context.Contatos.Remove(contato);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Contato {ContatoId} excluído.", id);
            return true;
        }

        private ContatoResponse MapearParaResponse(Contato contato)
        {
            return new ContatoResponse
            {
                Id = contato.Id,
                Tipo = contato.Tipo,
                Valor = contato.Valor,
                FkPessoaFisicaId = contato.FkPessoaFisicaId,
                FkPessoaJuridicaId = contato.FkPessoaJuridicaId
            };
        }
        private string LimparTelefone(string telefone)
        {
            return Regex.Replace(telefone ?? "", @"[^\d]", "");
        }
    }
}