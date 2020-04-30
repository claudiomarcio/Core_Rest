
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Domain.Models.Entities;

namespace Domain.EntityConfiguration.Mapping
{
    public class SistemaMap: IEntityTypeConfiguration<Sistema>
     {
        public void Configure(EntityTypeBuilder<Sistema> builder)
        {         
            {
               builder.ToTable("Sistema");
               builder.Property(x => x.Nome).HasMaxLength(20);
            }

        }
        
    }
}
