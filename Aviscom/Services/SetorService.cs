using Aviscom.Data;
using Aviscom.DTOs.Admin;
using Aviscom.Models.Usuario;
using Aviscom.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using NUlid;

namespace Aviscom.Services
{
    public class SetorService : ISetorService
    {
        private readonly AviscomContext _context;
        private readonly ILogger<SetorService> _logger;

        public SetorService(AviscomContext context, ILogger<SetorService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<SetorResponse> CreateSetorAsync(CreateSetorRequest request)
        {
            
            var existe = await _context.Setores.AnyAsync(s => s.Nome.ToLower() == request.Nome.ToLower());
            if (existe)
            {
                throw new InvalidOperationException($"Um setor com o nome '{request.Nome}' já existe.");
            }

            var novoSetor = new Setor
            {
                Nome = request.Nome
            };

            await _context.Setores.AddAsync(novoSetor);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Novo Setor {SetorId} criado com o nome: {Nome}", novoSetor.Id, novoSetor.Nome);
            return MapearParaResponse(novoSetor);
        }

        public async Task<IEnumerable<SetorResponse>> GetSetoresAsync()
        {
            return await _context.Setores
                .AsNoTracking()
                .Select(s => new SetorResponse
                {
                    Id = s.Id,
                    Nome = s.Nome
                })
                .ToListAsync();
        }

        public async Task<SetorResponse?> GetSetorByIdAsync(Ulid id)
        {
            return await _context.Setores
                .AsNoTracking()
                .Where(s => s.Id == id)
                .Select(s => new SetorResponse
                {
                    Id = s.Id,
                    Nome = s.Nome
                })
                .FirstOrDefaultAsync();
        }

        public async Task<SetorResponse?> UpdateSetorAsync(Ulid id, CreateSetorRequest request)
        {
            var setor = await _context.Setores.FindAsync(id);
            if (setor == null)
            {
                return null;
            }

            setor.Nome = request.Nome;
            await _context.SaveChangesAsync();

            _logger.LogInformation("Setor {SetorId} atualizado para o nome: {Nome}", setor.Id, setor.Nome);
            return MapearParaResponse(setor);
        }

        public async Task<bool> DeleteSetorAsync(Ulid id)
        {
            var setor = await _context.Setores.FindAsync(id);
            if (setor == null)
            {
                return false;
            }

            // Atenção: Adicionar lógica futura para verificar se o setor
            // está a ser usado por algum 'UsuarioFuncao' antes de apagar.
            _context.Setores.Remove(setor);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Setor {SetorId} excluído.", id);
            return true;
        }

        // --- Método Auxiliar ---
        private SetorResponse MapearParaResponse(Setor setor)
        {
            return new SetorResponse
            {
                Id = setor.Id,
                Nome = setor.Nome
            };
        }
    }
}
