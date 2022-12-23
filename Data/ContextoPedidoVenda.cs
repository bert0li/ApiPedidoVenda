using ApiPedidoVenda.Data.Map;
using ApiPedidoVenda.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiPedidoVenda.Data
{
    public class ContextoPedidoVenda : DbContext
    {
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Produto> Produtos { get; set; }
        public DbSet<Pedido> Pedidos { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
           => optionsBuilder.UseSqlite(@"DataSource=app.db;Cache=Shared")
                            .LogTo(Console.Write);

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.ApplyConfiguration(new PedidoMap());
            //modelBuilder.ApplyConfiguration(new ProdutoMap());
            //modelBuilder.ApplyConfiguration(new ClienteMap());
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ContextoPedidoVenda).Assembly);
        }
    }
}