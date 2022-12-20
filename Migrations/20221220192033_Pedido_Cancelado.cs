using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApiPedidoVenda.Migrations
{
    /// <inheritdoc />
    public partial class PedidoCancelado : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Cancelado",
                table: "Pedido",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Cancelado",
                table: "Pedido");
        }
    }
}
