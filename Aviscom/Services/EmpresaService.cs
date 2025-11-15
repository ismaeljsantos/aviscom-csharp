using Aviscom.Data;
using Aviscom.DTOs.Usuario;
using Aviscom.Models.Usuario;
using Aviscom.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using NUlid;
using System.Text.RegularExpressions;

namespace Aviscom.Services
{
    public class EmpresaService : IEmpresaService
    {
        private readonly AviscomContext _context;
        private readonly ILogger<EmpresaService> _logger;

        public EmpresaService(AviscomContext context, ILogger<EmpresaService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<EmpresaResponse> CreateEmpresaAsync(CreateEmpresaRequest request)
        {
            // Opcional: Verificar duplicados pelo nome
            var existe = await _context.Empresas.AnyAsync(e => e.Nome.ToLower() == request.Nome.ToLower());
            if (existe)
            {
                throw new InvalidOperationException($"Uma empresa com o nome '{request.Nome}' já existe.");
            }

            var novaEmpresa = new Empresa
            {
                Nome = request.Nome,
                // Limpa o CNPJ (remove máscara) antes de salvar, se existir
                CNPJ = request.Cnpj != null ? LimparCnpj(request.Cnpj) : null
            };

            await _context.Empresas.AddAsync(novaEmpresa);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Nova Empresa {EmpresaId} criada com o nome: {Nome}", novaEmpresa.Id, novaEmpresa.Nome);
            return MapearParaResponse(novaEmpresa);
        }

        public async Task<IEnumerable<EmpresaResponse>> GetEmpresasAsync()
        {
            return await _context.Empresas
                .AsNoTracking()
                .Select(e => new EmpresaResponse
                {
                    Id = e.Id,
                    Nome = e.Nome,
                    Cnpj = e.CNPJ // Opcional: Adicionar máscara aqui se desejar
                })
                .ToListAsync();
        }

        public async Task<EmpresaResponse?> GetEmpresaByIdAsync(Ulid id)
        {
            return await _context.Empresas
                .AsNoTracking()
                .Where(e => e.Id == id)
                .Select(e => new EmpresaResponse
                {
                    Id = e.Id,
                    Nome = e.Nome,
                    Cnpj = e.CNPJ
                })
                .FirstOrDefaultAsync();
        }

        public async Task<EmpresaResponse?> UpdateEmpresaAsync(Ulid id, CreateEmpresaRequest request)
        {
            var empresa = await _context.Empresas.FindAsync(id);
            if (empresa == null)
            {
                return null;
            }

            empresa.Nome = request.Nome;
            empresa.CNPJ = request.Cnpj != null ? LimparCnpj(request.Cnpj) : null;

            await _context.SaveChangesAsync();

            _logger.LogInformation("Empresa {EmpresaId} atualizada para o nome: {Nome}", empresa.Id, empresa.Nome);
            return MapearParaResponse(empresa);
        }

        public async Task<bool> DeleteEmpresaAsync(Ulid id)
        {
            var empresa = await _context.Empresas.FindAsync(id);
            if (empresa == null)
            {
                return false;
            }

            // Atenção: Adicionar lógica futura para verificar se a empresa
            // está a ser usada por alguma 'ExperienciaProfissional' antes de apagar.
            _context.Empresas.Remove(empresa);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Empresa {EmpresaId} excluída.", id);
            return true;
        }

        // --- Métodos Auxiliares ---
        private EmpresaResponse MapearParaResponse(Empresa empresa)
        {
            return new EmpresaResponse
            {
                Id = empresa.Id,
                Nome = empresa.Nome,
                Cnpj = empresa.CNPJ // Opcional: Adicionar máscara aqui
            };
        }

        private string LimparCnpj(string cnpj)
        {
            // Remove tudo que não for dígito
            return Regex.Replace(cnpj ?? "", @"[^\d]", "");
        }
    }
}
