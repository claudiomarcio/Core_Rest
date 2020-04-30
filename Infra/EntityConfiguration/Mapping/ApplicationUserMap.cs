
// using Microsoft.EntityFrameworkCore.Metadata.Builders;
// using Microsoft.EntityFrameworkCore;
// using webapi.Models.Entities.IdentityModel;

// namespace webapi.EntityConfiguration.Mapping
// {
//     public class ApplicationUserMap: IEntityTypeConfiguration<ApplicationUser>
//      {
//         public void Configure(EntityTypeBuilder<ApplicationUser> builder)
//         {         
//                  {
//                    builder.ToTable("Usuario"); 

//                    builder.Property(e => e.Email).HasColumnName("Email");
//                    builder.Property(e => e.PhoneNumber).HasColumnName("Telefone");
//                    builder.Property(e => e.UserName).HasColumnName("NomeUsuario");
//                    builder.Property(e => e.Id).HasColumnName("ID");

                    
//                     // Each User can have many UserClaims
//                     builder.HasMany(e => e.Claims)
//                         .WithOne()
//                         .HasForeignKey(uc => uc.UserId)
//                         .IsRequired();

//                     // Each User can have many UserLogins
//                     builder.HasMany(e => e.Logins)
//                         .WithOne()
//                         .HasForeignKey(ul => ul.UserId)
//                         .IsRequired();

//                     // Each User can have many UserTokens
//                     builder.HasMany(e => e.Tokens)
//                         .WithOne()
//                         .HasForeignKey(ut => ut.UserId)
//                         .IsRequired();

//                     // Each User can have many entries in the UserRole join table
//                     builder.HasMany(e => e.UserRoles)
//                         .WithOne()
//                         .HasForeignKey(ur => ur.UserId)
//                         .IsRequired();
//                  }
    
//         }
        
//     }
// }
