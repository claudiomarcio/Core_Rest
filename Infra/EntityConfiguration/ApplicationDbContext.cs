using Domain.EntityConfiguration.Mapping;
using Domain.Models.Entities;
using Infra.EntityConfiguration.Mapping;
using Microsoft.EntityFrameworkCore;

namespace Infra.EntityConfiguration
{
    public class ApplicationDbContext : DbContext
    {

        public ApplicationDbContext(DbContextOptions options) : base(options) { }

        public ApplicationDbContext() { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=dasdasdasdadsa.database.windows.net,1433;Database=dadadasd;User ID=daddsa;Password=dada;Connection Timeout=300000;");
        }

        public DbSet<Usuario> Usuario { get; set; }
        public DbSet<Perfil> Perfil { get; set; }
        public DbSet<Menus_Acesso> Menus_Acesso { get; set; }
        public DbSet<Empresa> Empresa { get; set; }
        public DbSet<GrupoEmpresa> GrupoEmpresa { get; set; }
        public DbSet<Sistema> Sistema { get; set; }
      

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new UsuarioMap());            
            modelBuilder.ApplyConfiguration(new EmpresaMap());        
            modelBuilder.ApplyConfiguration(new GrupoEmpresaMap());
           
        }
    }
}