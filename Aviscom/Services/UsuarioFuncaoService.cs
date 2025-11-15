using Aviscom.Data;
using Aviscom.DTOs.Admin;
using Aviscom.Models.Usuario;
using Aviscom.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using NUlid;

namespace Aviscom.Services
{
    public class UsuarioFuncaoService : IUsuarioFuncaoService
    {
        private readonly AviscomContext _context;
        private readonly ILogger<UsuarioFuncaoService> _logger;

        public UsuarioFuncaoService(AviscomContext context, ILogger<UsuarioFuncaoService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<UsuarioFuncaoResponse> AssignFuncaoToPfAsync(AssignFuncaoRequest request)
        {
            var usuario = await _context.UsuariosFisicos.FindAsync(request.FkUsuarioPfId);
            if (usuario == null)
                throw new KeyNotFoundException($"Usuário Pessoa Física com ID {request.FkUsuarioPfId} não encontrado.");

            var funcao = await _context.Funcoes.FindAsync(request.FkFuncaoId);
            if (funcao == null)
                throw new KeyNotFoundException($"Função com ID {request.FkFuncaoId} não encontrada.");

            var setor = await _context.Setores.FindAsync(request.FkSetorId);
            if (setor == null)
                throw new KeyNotFoundException($"Setor com ID {request.FkSetorId} não encontrado.");

            var novaAssociacao = new UsuarioFuncao
            {
                FkPessoaFisicaId = request.FkUsuarioPfId, // Agora compila
                FkFuncaoId = request.FkFuncaoId,
                FkSetorId = request.FkSetorId,
                Descricao = request.Descricao
            };

            await _context.UsuariosFuncoes.AddAsync(novaAssociacao);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Função {FuncaoId} associada ao Usuário {UsuarioId} no Setor {SetorId}",
                request.FkFuncaoId, request.FkUsuarioPfId, request.FkSetorId);

            return new UsuarioFuncaoResponse
            {
                FkUsuarioPfId = usuario.Id,
                FkFuncaoId = funcao.Id,
                FkSetorId = setor.Id,
                Descricao = novaAssociacao.Descricao,
                NomeUsuario = usuario.Nome,
                TituloFuncao = funcao.Titulo,
                NomeSetor = setor.Nome
            };
        }

        public async Task<IEnumerable<UsuarioFuncaoResponse>> GetFuncoesByPfIdAsync(Ulid usuarioPfId)
        {
            return await _context.UsuariosFuncoes
                .AsNoTracking()
                .Where(uf => uf.FkPessoaFisicaId == usuarioPfId) // Agora compila
                .Include(uf => uf.UsuarioFisica)
                .Include(uf => uf.Funcao)
                .Include(uf => uf.Setor)
                .Select(uf => new UsuarioFuncaoResponse
                {
                    FkUsuarioPfId = uf.FkPessoaFisicaId.Value, // Linha 75 - Agora compila
                    FkFuncaoId = uf.FkFuncaoId,
                    FkSetorId = uf.FkSetorId,
                    Descricao = uf.Descricao,
                    NomeUsuario = uf.UsuarioFisica.Nome,
                    TituloFuncao = uf.Funcao.Titulo,
                    NomeSetor = uf.Setor.Nome
                })
                .ToListAsync();
        }

        public async Task<bool> RemoveFuncaoFromPfAsync(AssignFuncaoRequest request)
        {
            var associacao = await _context.UsuariosFuncoes.FirstOrDefaultAsync(uf =>
                uf.FkPessoaFisicaId == request.FkUsuarioPfId && // Agora compila
                uf.FkFuncaoId == request.FkFuncaoId &&
                uf.FkSetorId == request.FkSetorId
            );

            if (associacao == null)
            {
                return false;
            }

            _context.UsuariosFuncoes.Remove(associacao);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Função {FuncaoId} removida do Usuário {UsuarioId} no Setor {SetorId}",
                request.FkFuncaoId, request.FkUsuarioPfId, request.FkSetorId);
            return true;
        }
    }
}