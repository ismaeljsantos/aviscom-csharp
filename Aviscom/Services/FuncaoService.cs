using Aviscom.Data;
using Aviscom.DTOs.Admin;
using Aviscom.Models.Usuario;
using Aviscom.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using NUlid;

namespace Aviscom.Services
{
    public class FuncaoService : IFuncaoService
    {
        private readonly AviscomContext _context;
        private readonly ILogger<FuncaoService> _logger;

        public FuncaoService(AviscomContext context, ILogger<FuncaoService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<FuncaoService> CreateFuncaoAsync(CreateFuncaoRequest request)
        {
            var existe = await _context.Funcoes.AnyAsync(f => f.Titulo.ToLower() == request.Titulo.ToLower());
            if (existe)
            {
                throw new InvalidOperationException($"Uma função com o título '{request.Titulo}' já existe.");
            }

            var novaFuncao = new Funcao
            {
                Titulo = request.Titulo
            };

            await _context.Funcoes.AddAsync(novaFuncao);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Nova Função {FuncaoId} criada com o título: {Titulo}", novaFuncao.Id, novaFuncao.Titulo);
            return MapearParaResponse(novaFuncao);
        }

        public async Task<IEnumerable<FuncaoResponse>> GetFuncoesAsync()
        {
            return await _context.Funcoes
                .AsNoTracking()
                .Select(f => new FuncaoResponse
                {
                    Id = f.Id,
                    Titulo = f.Titulo
                })
                .ToListAsync();
        }

        public async Task<FuncaoResponse?> GetFuncaoByIdAsync(Ulid id)
        {
            return await _context.Funcoes
                .AsNoTracking()
                .Where(f => f.Id == id)
                .Select(f => new FuncaoResponse
                {
                    Id = f.Id,
                    Titulo = f.Titulo
                })
                .FirstOrDefaultAsync();
        }

        public async Task<FuncaoResponse?> UpdateFuncaoAsync(Ulid id, CreateFuncaoRequest request)
        {
            var funcao = await _context.Funcoes.FindAsync(id);
            if (funcao == null)
            {
                return null;
            }

            funcao.Titulo = request.Titulo;
            await _context.SaveChangesAsync();

            _logger.LogInformation("Função {FuncaoId} atualizada para o título: {Titulo}", funcao.Id, funcao.Titulo);
            return MapearParaResponse(funcao);
        }

        public async Task<bool> DeleteFuncaoAsync(Ulid id)
        {
            var funcao = await _context.Funcoes.FindAsync(id);
            if (funcao == null)
            {
                return false;
            }

            // Atenção: Adicionar lógica futura para verificar se a função
            // está a ser usada por algum 'UsuarioFuncao' antes de apagar.
            _context.Funcoes.Remove(funcao);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Função {FuncaoId} excluída.", id);
            return true;
        }

        private FuncaoResponse MapearParaResponse(Funcao funcao)
        {
            return new FuncaoResponse
            {
                Id = funcao.Id,
                Titulo = funcao.Titulo
            };
        }
    }
}
