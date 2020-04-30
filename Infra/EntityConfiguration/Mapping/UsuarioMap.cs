
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Domain.Models.Entities;

namespace Domain.EntityConfiguration.Mapping
{
    public class UsuarioMap: IEntityTypeConfiguration<Usuario>
     {
        public void Configure(EntityTypeBuilder<Usuario> builder)
        {         
            {
               builder.ToTable("Usuario");
               builder.Property(x => x.Nome).HasMaxLength(100);
                builder.Property(x => x.Email).HasMaxLength(100);
                builder.Property(x => x.ativo).IsRequired();
            }

        }
        
    }
}
