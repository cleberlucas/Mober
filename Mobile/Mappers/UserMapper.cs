using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Mobile.Models;

namespace Mobile.Mappers
{
    public class UserMapper : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("usuario");
            //builder.HasKey(p => new { p.Id });
            builder.Property(p => p.Name).HasColumnName("nome");
            builder.Property(p => p.Phone).HasColumnName("telefone");
            builder.Property(p => p.Longitude).HasColumnName("longitude");
            builder.Property(p => p.Latitude).HasColumnName("latitude");
            builder.Property(p => p.Service).HasColumnName("servico");
            builder.Property(p => p.Servant).HasColumnName("prestador");
            builder.Property(p => p.Updated).HasColumnName("atualizado");
        }
    }
}
