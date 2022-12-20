using ApiPedidoVenda.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ApiPedidoVenda.Data.Map
{
    public class ClienteMap : IEntityTypeConfiguration<Cliente>
    {
        public void Configure(EntityTypeBuilder<Cliente> builder)
        {
            builder.ToTable("Cliente");

            builder.HasKey(k => k.Id);

            builder.Property(p => p.Id)
                   .ValueGeneratedOnAdd();

            builder.Property(p => p.Nome)
                   .IsRequired()
                   .HasColumnName("Nome")
                   .HasColumnType("NVARCHAR")
                   .HasMaxLength(80);

            builder.Property(p => p.Email)
                   .IsRequired()
                   .HasColumnName("Email")
                   .HasColumnType("NVARCHAR")
                   .HasMaxLength(80);
        }
    }
}