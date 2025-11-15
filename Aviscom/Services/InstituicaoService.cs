using Aviscom.Data;
using Aviscom.DTOs.Usuario;
using Aviscom.Models.Usuario;
using Aviscom.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using NUlid;
using System.Text.RegularExpressions;

namespace Aviscom.Services
{
    public class InstituicaoService : IInstituicaoService
    {
        private readonly AviscomContext _context;
        private readonly ILogger<InstituicaoService> _logger;

        public InstituicaoService(AviscomContext context, ILogger<InstituicaoService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<InstituicaoResponse> CreateInstituicaoAsync(CreateInstituicaoRequest request)
        {
            // Opcional: Verificar duplicados pelo nome
            var existe = await _context.Instituicoes.AnyAsync(i => i.Nome.ToLower() == request.Nome.ToLower());
            if (existe)
            {
                throw new InvalidOperationException($"Uma instituição com o nome '{request.Nome}' já existe.");
            }

            var novaInstituicao = new Instituicao
            {
                Nome = request.Nome,
                // Limpa o CNPJ (remove máscara) antes de salvar, se existir
                CNPJ = request.Cnpj != null ? LimparCnpj(request.Cnpj) : null
            };

            await _context.Instituicoes.AddAsync(novaInstituicao);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Nova Instituição {InstituicaoId} criada com o nome: {Nome}", novaInstituicao.Id, novaInstituicao.Nome);
            return MapearParaResponse(novaInstituicao);
        }

        public async Task<IEnumerable<InstituicaoResponse>> GetInstituicoesAsync()
        {
            return await _context.Instituicoes
                .AsNoTracking()
                .Select(i => new InstituicaoResponse
                {
                    Id = i.Id,
                    Nome = i.Nome,
                    Cnpj = i.CNPJ
                })
                .ToListAsync();
        }

        public async Task<InstituicaoResponse?> GetInstituicaoByIdAsync(Ulid id)
        {
            return await _context.Instituicoes
                .AsNoTracking()
                .Where(i => i.Id == id)
                .Select(i => new InstituicaoResponse
                {
                    Id = i.Id,
                    Nome = i.Nome,
                    Cnpj = i.CNPJ
                })
                .FirstOrDefaultAsync();
        }

        public async Task<InstituicaoResponse?> UpdateInstituicaoAsync(Ulid id, CreateInstituicaoRequest request)
        {
            var instituicao = await _context.Instituicoes.FindAsync(id);
            if (instituicao == null)
            {
                return null;
            }

            instituicao.Nome = request.Nome;
            instituicao.CNPJ = request.Cnpj != null ? LimparCnpj(request.Cnpj) : null;

            await _context.SaveChangesAsync();

            _logger.LogInformation("Instituição {InstituicaoId} atualizada para o nome: {Nome}", instituicao.Id, instituicao.Nome);
            return MapearParaResponse(instituicao);
        }

        public async Task<bool> DeleteInstituicaoAsync(Ulid id)
        {
            var instituicao = await _context.Instituicoes.FindAsync(id);
            if (instituicao == null)
            {
                return false;
            }

            // Atenção: Adicionar lógica futura para verificar se a instituição
            // está a ser usada por alguma 'Escolaridade' antes de apagar.
            _context.Instituicoes.Remove(instituicao);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Instituição {InstituicaoId} excluída.", id);
            return true;
        }

        // --- Métodos Auxiliares ---
        private InstituicaoResponse MapearParaResponse(Instituicao instituicao)
        {
            return new InstituicaoResponse
            {
                Id = instituicao.Id,
                Nome = instituicao.Nome,
                Cnpj = instituicao.CNPJ
            };
        }

        private string LimparCnpj(string cnpj)
        {
            // Remove tudo que não for dígito
            return Regex.Replace(cnpj ?? "", @"[^\d]", "");
        }
    }
}
