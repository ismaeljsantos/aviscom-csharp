using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NUlid;
using Aviscom.Models;
using Aviscom.Models.Usuario;

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
            // === Converter TODOS os Ulid para string(26) ===
            var ulidToStringConverter = new ValueConverter<Ulid, string>(
                v => v.ToString(),
                v => Ulid.Parse(v));

            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                foreach (var property in entityType.GetProperties())
                {
                    if (property.ClrType == typeof(Ulid))
                    {
                        property.SetColumnType("char(26)");
                        property.SetValueConverter(ulidToStringConverter);
                    }
                }
            }

            // === Chave composta para UsuarioFuncao ===
            modelBuilder.Entity<UsuarioFuncao>()
                .HasKey(uf => new { uf.FkUsuarioId, uf.FkFuncaoId, uf.FkSetorId });

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

            // === Endereço: 1:1 com PF ou PJ (opcional) ===
            modelBuilder.Entity<Endereco>()
                .HasIndex(e => e.FkPessoaFisicaId)
                .IsUnique();

            modelBuilder.Entity<Endereco>()
                .HasIndex(e => e.FkPessoaJuridicaId)
                .IsUnique();
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