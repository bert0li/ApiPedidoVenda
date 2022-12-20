using ApiPedidoVenda.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ApiPedidoVenda.Data.Map
{
    public class PedidoMap : IEntityTypeConfiguration<Pedido>
    {
        public void Configure(EntityTypeBuilder<Pedido> builder)
        {
            builder.ToTable("Pedido");

            builder.HasKey(k => k.Id);

            builder.Property(p => p.Id)
                   .ValueGeneratedOnAdd();

            builder.Property(p => p.ValorTotal)
                   .IsRequired()
                   .HasColumnName("ValorTotal")
                   .HasColumnType("decimal(18, 2)");

            builder.Property(p => p.DataPedido)
                   .IsRequired()
                   .HasColumnName("DataPedido")
                   .HasColumnType("SMALLDATETIME")
                   .HasDefaultValueSql("GETDATE()");

            builder.Property(p => p.Cancelado)
                   .HasColumnName("Cancelado");

            builder.HasOne(o => o.Cliente)
                   .WithMany(m => m.Pedidos)
                   .HasConstraintName("FK_Pedido_Cliente")
                   .OnDelete(DeleteBehavior.NoAction);

            builder.HasMany(m => m.Itens)
                   .WithMany(m => m.Pedidos)
                   .UsingEntity<Dictionary<string, object>>
                   (
                        "PedidoItens",
                        pedido => pedido.HasOne<Produto>()
                                        .WithMany()
                                        .HasForeignKey("PedidoId")
                                        .HasConstraintName("FK_PedidoItens_PedidoId")
                                        .OnDelete(DeleteBehavior.NoAction),
                        produto => produto.HasOne<Pedido>()
                                          .WithMany()
                                          .HasForeignKey("ProdutoId")
                                          .HasConstraintName("FK_PedidoItens_ProdutoId")
                                          .OnDelete(DeleteBehavior.NoAction)
                   );
        }
    }
}