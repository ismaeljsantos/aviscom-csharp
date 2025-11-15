// Aviscom.Data/AviscomContext.cs
using Microsoft.EntityFrameworkCore;
using Aviscom.Models;
using Aviscom.Models.Usuario;
using Aviscom.Data.Configurations;

namespace Aviscom.Data
{
    public class AviscomContext : DbContext
    {
        public AviscomContext(DbContextOptions<AviscomContext> options) : base(options) { }

        public DbSet<UsuarioPessoaFisica> UsuariosFisicos { get; set; }
        public DbSet<UsuarioPessoaJuridica> UsuariosJuridicos { get; set; }
        public DbSet<Endereco> Enderecos { get; set; }
        public DbSet<Contato> Contatos { get; set; }
        public DbSet<Escolaridade> Escolaridades { get; set; }
        public DbSet<ExperienciaProfissional> ExperienciasProfissionais { get; set; }
        public DbSet<Empresa> Empresas { get; set; }
        public DbSet<Funcao> Funcoes { get; set; }
        public DbSet<Setor> Setores { get; set; }
        public DbSet<Instituicao> Instituicoes { get; set; }
        public DbSet<UsuarioFuncao> UsuariosFuncoes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Aplica todas as configurações
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AviscomContext).Assembly);

            // === Chave composta para UsuarioFuncao ===
            //modelBuilder.Entity<UsuarioFuncao>()
            //    .HasKey(uf => new {
            //        uf.FkPessoaFisicaId, // Corrigido
            //        uf.FkPessoaJuridicaId, // Corrigido
            //        uf.FkFuncaoId,
            //        uf.FkSetorId
            //    });

            // === Índices únicos ===
            modelBuilder.Entity<UsuarioPessoaFisica>()
                .HasIndex(u => u.CpfHash)
                .IsUnique();

            modelBuilder.Entity<UsuarioPessoaJuridica>()
                .HasIndex(u => u.CnpjHash)
                .IsUnique();

            // === Relacionamento Responsável ===
            modelBuilder.Entity<UsuarioPessoaJuridica>()
                .HasOne(pj => pj.Responsavel)
                .WithMany(pf => pf.EmpresasResponsavel)
                .HasForeignKey(pj => pj.FkResponsavelId)
                .OnDelete(DeleteBehavior.Restrict);
        }

        public override int SaveChanges()
        {
            UpdateTimestamps();
            return base.SaveChanges();
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            UpdateTimestamps();
            return await base.SaveChangesAsync(cancellationToken);
        }

        private void UpdateTimestamps()
        {
            var entries = ChangeTracker
                .Entries<BaseEntity>()
                .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified);

            foreach (var entry in entries)
            {
                entry.Entity.DataAtualizacao = DateTime.UtcNow;
                if (entry.State == EntityState.Added)
                {
                    entry.Entity.DataCriacao = DateTime.UtcNow;
                }
            }
        }
    }
}