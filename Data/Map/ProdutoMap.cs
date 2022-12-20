using ApiPedidoVenda.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ApiPedidoVenda.Data.Map
{
    public class ProdutoMap : IEntityTypeConfiguration<Produto>
    {
        public void Configure(EntityTypeBuilder<Produto> builder)
        {
            builder.ToTable("Produto");

            builder.HasKey(k => k.Id);

            builder.Property(p => p.Id)
                   .ValueGeneratedOnAdd();
            
            builder.Property(p => p.Nome)
                   .IsRequired()
                   .HasColumnName("Nome")
                   .HasColumnType("NVARCHAR")
                   .HasMaxLength(80);
            
            builder.Property(p => p.Valor)
                   .IsRequired()
                   .HasColumnName("Valor")
                   .HasColumnType("decimal(18, 2)");    
            
            builder.Property(p => p.Ativo)
                   .IsRequired()
                   .HasColumnName("Ativo");
        }
    }
}