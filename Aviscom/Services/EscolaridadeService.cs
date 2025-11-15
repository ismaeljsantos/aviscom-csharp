using Aviscom.Data;
using Aviscom.DTOs.Usuario;
using Aviscom.Models.Usuario;
using Aviscom.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using NUlid;

namespace Aviscom.Services
{
    public class EscolaridadeService : IEscolaridadeService
    {
        private readonly AviscomContext _context;
        private readonly ILogger<EscolaridadeService> _logger;

        public EscolaridadeService(AviscomContext context, ILogger<EscolaridadeService> logger)
        {
            _context = context;
            _logger = logger;
        }

        // --- Métodos Aninhados ---

        public async Task<EscolaridadeResponse> CreateEscolaridadeParaPfAsync(Ulid usuarioPfId, CreateEscolaridadeRequest request)
        {
            // 1. Validar se o Utilizador e a Instituição existem
            var usuario = await _context.UsuariosFisicos.FindAsync(usuarioPfId);
            if (usuario == null)
                throw new KeyNotFoundException($"Utilizador Pessoa Física com ID {usuarioPfId} não encontrado.");

            var instituicao = await _context.Instituicoes.FindAsync(request.FkInstituicaoId);
            if (instituicao == null)
                throw new KeyNotFoundException($"Instituição com ID {request.FkInstituicaoId} não encontrada.");

            // 2. Mapear DTO para Entidade
            var novaEscolaridade = new Escolaridade
            {
                Tipo = request.Tipo,
                NomeCurso = request.NomeCurso,
                AnoInicio = request.AnoInicio,
                AnoConclusao = request.AnoConclusao,
                Ativo = request.Ativo,
                FkUsuarioId = usuarioPfId, // Associa ao Utilizador
                FkInstituicaoId = request.FkInstituicaoId // Associa à Instituição
            };

            // 3. Salvar
            await _context.Escolaridades.AddAsync(novaEscolaridade);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Nova Escolaridade {Id} criada para o utilizador {UsuarioId}", novaEscolaridade.Id, usuarioPfId);

            // 4. Retornar DTO de Resposta (com nome da instituição)
            return await MapearParaResponseComNomes(novaEscolaridade);
        }

        public async Task<IEnumerable<EscolaridadeResponse>> GetEscolaridadesByPfIdAsync(Ulid usuarioPfId)
        {
            return await _context.Escolaridades
                .AsNoTracking()
                .Where(e => e.FkUsuarioId == usuarioPfId)
                .Include(e => e.Instituicao) // Inclui a Instituição para obter o nome
                .Select(e => new EscolaridadeResponse
                {
                    Id = e.Id,
                    Tipo = e.Tipo,
                    NomeCurso = e.NomeCurso,
                    AnoInicio = e.AnoInicio,
                    AnoConclusao = e.AnoConclusao,
                    Ativo = e.Ativo,
                    FkUsuarioId = e.FkUsuarioId,
                    FkInstituicaoId = e.FkInstituicaoId,
                    NomeInstituicao = e.Instituicao.Nome
                })
                .ToListAsync();
        }

        // --- Métodos Diretos ---

        public async Task<EscolaridadeResponse?> GetEscolaridadeByIdAsync(Ulid id)
        {
            var escolaridade = await _context.Escolaridades
                .AsNoTracking()
                .Where(e => e.Id == id)
                .Include(e => e.Instituicao)
                .FirstOrDefaultAsync();

            return escolaridade != null ? MapearParaResponse(escolaridade, escolaridade.Instituicao.Nome) : null;
        }

        public async Task<EscolaridadeResponse?> UpdateEscolaridadeAsync(Ulid id, CreateEscolaridadeRequest request)
        {
            var escolaridade = await _context.Escolaridades.FindAsync(id);
            if (escolaridade == null)
                return null; // Não encontrado

            // Validar a nova instituição, se ela mudou
            if (escolaridade.FkInstituicaoId != request.FkInstituicaoId)
            {
                var novaInstituicao = await _context.Instituicoes.FindAsync(request.FkInstituicaoId);
                if (novaInstituicao == null)
                    throw new KeyNotFoundException($"Nova Instituição com ID {request.FkInstituicaoId} não encontrada.");
            }

            // Mapeamento
            escolaridade.Tipo = request.Tipo;
            escolaridade.NomeCurso = request.NomeCurso;
            escolaridade.AnoInicio = request.AnoInicio;
            escolaridade.AnoConclusao = request.AnoConclusao;
            escolaridade.Ativo = request.Ativo;
            escolaridade.FkInstituicaoId = request.FkInstituicaoId;

            await _context.SaveChangesAsync();
            _logger.LogInformation("Escolaridade {Id} atualizada.", id);

            return await MapearParaResponseComNomes(escolaridade);
        }

        public async Task<bool> DeleteEscolaridadeAsync(Ulid id)
        {
            var escolaridade = await _context.Escolaridades.FindAsync(id);
            if (escolaridade == null)
            {
                return false;
            }

            _context.Escolaridades.Remove(escolaridade);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Escolaridade {Id} excluída.", id);
            return true;
        }

        // --- Métodos Auxiliares ---

        // Mapeamento simples
        private EscolaridadeResponse MapearParaResponse(Escolaridade e, string nomeInstituicao)
        {
            return new EscolaridadeResponse
            {
                Id = e.Id,
                Tipo = e.Tipo,
                NomeCurso = e.NomeCurso,
                AnoInicio = e.AnoInicio,
                AnoConclusao = e.AnoConclusao,
                Ativo = e.Ativo,
                FkUsuarioId = e.FkUsuarioId,
                FkInstituicaoId = e.FkInstituicaoId,
                NomeInstituicao = nomeInstituicao
            };
        }

        // Mapeamento que busca o nome da instituição (útil após Crate/Update)
        private async Task<EscolaridadeResponse> MapearParaResponseComNomes(Escolaridade e)
        {
            var nomeInstituicao = await _context.Instituicoes
                                        .Where(i => i.Id == e.FkInstituicaoId)
                                        .Select(i => i.Nome)
                                        .FirstOrDefaultAsync();

            return MapearParaResponse(e, nomeInstituicao ?? "Instituição não encontrada");
        }
    }
}
