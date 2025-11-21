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

        // ===================================
        // === MÉTODOS PARA PESSOA JURÍDICA ==
        // ===================================

        public async Task<UsuarioFuncaoResponse> AssignFuncaoToPjAsync(AssignFuncaoPjRequest request)
        {
            // 1. Validar se as entidades (Usuário PJ, Função, Setor) existem
            var usuario = await _context.UsuariosJuridicos.FindAsync(request.FkUsuarioPjId);
            if (usuario == null)
                throw new KeyNotFoundException($"Usuário Pessoa Jurídica com ID {request.FkUsuarioPjId} não encontrado.");

            var funcao = await _context.Funcoes.FindAsync(request.FkFuncaoId);
            if (funcao == null)
                throw new KeyNotFoundException($"Função com ID {request.FkFuncaoId} não encontrada.");

            var setor = await _context.Setores.FindAsync(request.FkSetorId);
            if (setor == null)
                throw new KeyNotFoundException($"Setor com ID {request.FkSetorId} não encontrado.");

            // 2. Criar a nova associação, definindo FkPessoaJuridicaId
            var novaAssociacao = new UsuarioFuncao
            {
                FkPessoaJuridicaId = request.FkUsuarioPjId, // Usamos o ID do PJ
                FkFuncaoId = request.FkFuncaoId,
                FkSetorId = request.FkSetorId,
                Descricao = request.Descricao
            };

            // 3. Adicionar e Salvar
            await _context.UsuariosFuncoes.AddAsync(novaAssociacao);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Função {FuncaoId} associada ao Usuário PJ {UsuarioId} no Setor {SetorId}",
                request.FkFuncaoId, request.FkUsuarioPjId, request.FkSetorId);

            // 4. Retornar a resposta, usando RazaoSocial para o NomeUsuario
            return new UsuarioFuncaoResponse
            {
                FkUsuarioPjId = usuario.Id,
                FkFuncaoId = funcao.Id,
                FkSetorId = setor.Id,
                Descricao = novaAssociacao.Descricao,
                NomeUsuario = usuario.RazaoSocial,
                TituloFuncao = funcao.Titulo,
                NomeSetor = setor.Nome
            };
        }

        public async Task<IEnumerable<UsuarioFuncaoResponse>> GetFuncoesByPjIdAsync(Ulid usuarioPjId)
        {
            return await _context.UsuariosFuncoes
                .AsNoTracking()
                .Where(uf => uf.FkPessoaJuridicaId == usuarioPjId)
                .Include(uf => uf.UsuarioJuridica)
                .Include(uf => uf.Funcao)
                .Include(uf => uf.Setor)
                .Select(uf => new UsuarioFuncaoResponse
                {
                    FkUsuarioPjId = uf.FkPessoaJuridicaId,
                    FkFuncaoId = uf.FkFuncaoId,
                    FkSetorId = uf.FkSetorId,
                    Descricao = uf.Descricao,
                    NomeUsuario = uf.UsuarioJuridica.RazaoSocial,
                    TituloFuncao = uf.Funcao.Titulo,
                    NomeSetor = uf.Setor.Nome
                })
                .ToListAsync();
        }

        public async Task<bool> RemoveFuncaoFromPjAsync(AssignFuncaoPjRequest request)
        {
            // Encontra a associação pela chave composta
            var associacao = await _context.UsuariosFuncoes.FirstOrDefaultAsync(uf =>
                uf.FkPessoaJuridicaId == request.FkUsuarioPjId &&
                uf.FkFuncaoId == request.FkFuncaoId &&
                uf.FkSetorId == request.FkSetorId
            );

            if (associacao == null)
            {
                return false; // Associação não encontrada
            }

            _context.UsuariosFuncoes.Remove(associacao);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Função {FuncaoId} removida do Usuário PJ {UsuarioId} no Setor {SetorId}",
                request.FkFuncaoId, request.FkUsuarioPjId, request.FkSetorId);
            return true;
        }

        // ====================================================
        // === MÉTODO PARA LISTAR POR FUNÇÃO SOMERNTE ADMIN ===
        // ====================================================
        public async Task<IEnumerable<FuncaoAssociacaoResponse>> GetUsuariosByFuncaoTituloAsync(string tituloFuncao)
        {
            // 1. Buscar a função pelo título (ignorando maiúsculas/minúsculas)
            var funcao = await _context.Funcoes
                .AsNoTracking()
                .Where(f => f.Titulo.ToLower() == tituloFuncao.ToLower() && f.IsAtivo)
                .FirstOrDefaultAsync();

            if (funcao == null)
            {
                // NOVO: Lançar exceção, delegando o tratamento do 404 ao Controller.
                throw new KeyNotFoundException($"Função com título '{tituloFuncao}' não encontrada ou inativa.");
            }

            var funcaoId = funcao.Id;

            // 2. Consulta para Pessoas Físicas (PF)
            var usuariosPf = await _context.UsuariosFuncoes
                .AsNoTracking()
                .Where(uf => uf.FkFuncaoId == funcaoId && uf.IsAtivo)
                .Include(uf => uf.UsuarioFisica)
                .Include(uf => uf.Setor)
                .Where(uf => uf.UsuarioFisica.IsAtivo) // Filtro Soft Delete para o utilizador PF
                .Select(uf => new FuncaoAssociacaoResponse
                {
                    UsuarioId = uf.FkPessoaFisicaId.Value,
                    Nome = uf.UsuarioFisica.Nome,
                    TipoUsuario = "PF",
                    DescricaoFuncao = uf.Descricao,
                    NomeSetor = uf.Setor.Nome
                })
                .ToListAsync();

            // 3. Consulta para Pessoas Jurídicas (PJ)
            var usuariosPj = await _context.UsuariosFuncoes
                .AsNoTracking()
                .Where(uf => uf.FkFuncaoId == funcaoId && uf.IsAtivo)
                .Include(uf => uf.UsuarioJuridica)
                .Include(uf => uf.Setor)
                .Where(uf => uf.UsuarioJuridica.IsAtivo) // Filtro Soft Delete para o utilizador PJ
                .Select(uf => new FuncaoAssociacaoResponse
                {
                    UsuarioId = uf.FkPessoaJuridicaId.Value,
                    Nome = uf.UsuarioJuridica.RazaoSocial,
                    TipoUsuario = "PJ",
                    DescricaoFuncao = uf.Descricao,
                    NomeSetor = uf.Setor.Nome
                })
                .ToListAsync();

            // 4. Combina e retorna as duas listas
            return usuariosPf.Concat(usuariosPj);
        }
    }
}