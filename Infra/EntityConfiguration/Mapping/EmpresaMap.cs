
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Domain.Models.Entities;

namespace Infra.EntityConfiguration.Mapping
{
    public class EmpresaMap : IEntityTypeConfiguration<Empresa>
     {
        public void Configure(EntityTypeBuilder<Empresa> builder)
        {         
            {
              builder.ToTable("Empresa");
              builder.HasIndex(x => x.IdExterno);
              builder.HasIndex(x => x.SistemaId);
              builder.Property(x => x.Nome).HasMaxLength(100);
            }
        }

        
    }
}
