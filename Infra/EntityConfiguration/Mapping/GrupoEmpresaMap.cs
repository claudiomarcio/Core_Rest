
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Domain.Models.Entities;

namespace Domain.EntityConfiguration.Mapping
{
    public class GrupoEmpresaMap : IEntityTypeConfiguration<GrupoEmpresa>
     {
        public void Configure(EntityTypeBuilder<GrupoEmpresa> builder)
        {         
            {
               builder.ToTable("GrupoEmpresa");
               builder.HasKey(x => new { x.GrupoEmpresaId });
            }
        }       
     }
}
