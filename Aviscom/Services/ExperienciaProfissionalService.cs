using Aviscom.Data;
using Aviscom.DTOs.Usuario;
using Aviscom.Models.Usuario;
using Aviscom.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using NUlid;

namespace Aviscom.Services
{
    public class ExperienciaProfissionalService : IExperienciaProfissionalService
    {
        private readonly AviscomContext _context;
        private readonly ILogger<ExperienciaProfissionalService> _logger;

        public ExperienciaProfissionalService(AviscomContext context, ILogger<ExperienciaProfissionalService> logger)
        {
            _context = context;
            _logger = logger;
        }

        // --- Métodos Aninhados ---

        public async Task<ExperienciaProfissionalResponse> CreateExperienciaParaPfAsync(Ulid usuarioPfId, CreateExperienciaProfissionalRequest request)
        {
            // 1. Validar se o Utilizador e a Empresa existem
            var usuario = await _context.UsuariosFisicos.FindAsync(usuarioPfId);
            if (usuario == null)
                throw new KeyNotFoundException($"Utilizador Pessoa Física com ID {usuarioPfId} não encontrado.");

            var empresa = await _context.Empresas.FindAsync(request.FkEmpresaId);
            if (empresa == null)
                throw new KeyNotFoundException($"Empresa com ID {request.FkEmpresaId} não encontrada.");

            // 2. Mapear DTO para Entidade
            var novaExperiencia = new ExperienciaProfissional
            {
                Cargo = request.Cargo,
                AnoEntrada = request.AnoEntrada,
                AnoSaida = request.AnoSaida,
                DescricaoAtividades = request.DescricaoAtividades,
                FkUsuarioId = usuarioPfId, // Associa ao Utilizador
                FkEmpresaId = request.FkEmpresaId // Associa à Empresa
            };

            // 3. Salvar
            await _context.ExperienciasProfissionais.AddAsync(novaExperiencia);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Nova Experiência Profissional {Id} criada para o utilizador {UsuarioId}", novaExperiencia.Id, usuarioPfId);

            // 4. Retornar DTO de Resposta (com nome da empresa)
            return await MapearParaResponseComNomes(novaExperiencia);
        }

        public async Task<IEnumerable<ExperienciaProfissionalResponse>> GetExperienciasByPfIdAsync(Ulid usuarioPfId)
        {
            return await _context.ExperienciasProfissionais
                .AsNoTracking()
                .Where(e => e.FkUsuarioId == usuarioPfId)
                .Include(e => e.Empresa) // Inclui a Empresa para obter o nome
                .Select(e => new ExperienciaProfissionalResponse
                {
                    Id = e.Id,
                    Cargo = e.Cargo,
                    AnoEntrada = e.AnoEntrada,
                    AnoSaida = e.AnoSaida,
                    DescricaoAtividades = e.DescricaoAtividades,
                    FkUsuarioId = e.FkUsuarioId,
                    FkEmpresaId = e.FkEmpresaId,
                    NomeEmpresa = e.Empresa.Nome
                })
                .ToListAsync();
        }

        // --- Métodos Diretos ---

        public async Task<ExperienciaProfissionalResponse?> GetExperienciaByIdAsync(Ulid id)
        {
            var experiencia = await _context.ExperienciasProfissionais
                .AsNoTracking()
                .Where(e => e.Id == id)
                .Include(e => e.Empresa)
                .FirstOrDefaultAsync();

            return experiencia != null ? MapearParaResponse(experiencia, experiencia.Empresa.Nome) : null;
        }

        public async Task<ExperienciaProfissionalResponse?> UpdateExperienciaAsync(Ulid id, CreateExperienciaProfissionalRequest request)
        {
            var experiencia = await _context.ExperienciasProfissionais.FindAsync(id);
            if (experiencia == null)
                return null; // Não encontrado

            // Validar a nova empresa, se ela mudou
            if (experiencia.FkEmpresaId != request.FkEmpresaId)
            {
                var novaEmpresa = await _context.Empresas.FindAsync(request.FkEmpresaId);
                if (novaEmpresa == null)
                    throw new KeyNotFoundException($"Nova Empresa com ID {request.FkEmpresaId} não encontrada.");
            }

            // Mapeamento
            experiencia.Cargo = request.Cargo;
            experiencia.AnoEntrada = request.AnoEntrada;
            experiencia.AnoSaida = request.AnoSaida;
            experiencia.DescricaoAtividades = request.DescricaoAtividades;
            experiencia.FkEmpresaId = request.FkEmpresaId;

            await _context.SaveChangesAsync();
            _logger.LogInformation("Experiência Profissional {Id} atualizada.", id);

            return await MapearParaResponseComNomes(experiencia);
        }

        public async Task<bool> DeleteExperienciaAsync(Ulid id)
        {
            var experiencia = await _context.ExperienciasProfissionais.FindAsync(id);
            if (experiencia == null)
            {
                return false;
            }

            _context.ExperienciasProfissionais.Remove(experiencia);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Experiência Profissional {Id} excluída.", id);
            return true;
        }

        // --- Métodos Auxiliares ---

        // Mapeamento simples
        private ExperienciaProfissionalResponse MapearParaResponse(ExperienciaProfissional e, string nomeEmpresa)
        {
            return new ExperienciaProfissionalResponse
            {
                Id = e.Id,
                Cargo = e.Cargo,
                AnoEntrada = e.AnoEntrada,
                AnoSaida = e.AnoSaida,
                DescricaoAtividades = e.DescricaoAtividades,
                FkUsuarioId = e.FkUsuarioId,
                FkEmpresaId = e.FkEmpresaId,
                NomeEmpresa = nomeEmpresa
            };
        }

        // Mapeamento que busca o nome da empresa (útil após Crate/Update)
        private async Task<ExperienciaProfissionalResponse> MapearParaResponseComNomes(ExperienciaProfissional e)
        {
            var nomeEmpresa = await _context.Empresas
                                        .Where(emp => emp.Id == e.FkEmpresaId)
                                        .Select(emp => emp.Nome)
                                        .FirstOrDefaultAsync();

            return MapearParaResponse(e, nomeEmpresa ?? "Empresa não encontrada");
        }
    }
}
