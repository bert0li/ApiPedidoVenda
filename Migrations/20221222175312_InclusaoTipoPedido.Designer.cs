﻿// <auto-generated />
using System;
using ApiPedidoVenda.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace ApiPedidoVenda.Migrations
{
    [DbContext(typeof(ContextoPedidoVenda))]
    [Migration("20221222175312_InclusaoTipoPedido")]
    partial class InclusaoTipoPedido
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "7.0.1");

            modelBuilder.Entity("ApiPedidoVenda.Models.Cliente", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(80)
                        .HasColumnType("NVARCHAR")
                        .HasColumnName("Email");

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasMaxLength(80)
                        .HasColumnType("NVARCHAR")
                        .HasColumnName("Nome");

                    b.HasKey("Id");

                    b.ToTable("Cliente", (string)null);
                });

            modelBuilder.Entity("ApiPedidoVenda.Models.Pedido", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<bool>("Cancelado")
                        .HasColumnType("INTEGER")
                        .HasColumnName("Cancelado");

                    b.Property<int?>("ClienteId")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("DataPedido")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("SMALLDATETIME")
                        .HasColumnName("DataPedido")
                        .HasDefaultValueSql("GETDATE()");

                    b.Property<int>("TipoPedido")
                        .HasColumnType("INTEGER")
                        .HasColumnName("TipoPedido");

                    b.Property<decimal>("ValorTotal")
                        .HasColumnType("decimal(18, 2)")
                        .HasColumnName("ValorTotal");

                    b.HasKey("Id");

                    b.HasIndex("ClienteId");

                    b.ToTable("Pedido", (string)null);
                });

            modelBuilder.Entity("ApiPedidoVenda.Models.Produto", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<bool>("Ativo")
                        .HasColumnType("INTEGER")
                        .HasColumnName("Ativo");

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasMaxLength(80)
                        .HasColumnType("NVARCHAR")
                        .HasColumnName("Nome");

                    b.Property<decimal>("Valor")
                        .HasColumnType("decimal(18, 2)")
                        .HasColumnName("Valor");

                    b.HasKey("Id");

                    b.ToTable("Produto", (string)null);
                });

            modelBuilder.Entity("PedidoItens", b =>
                {
                    b.Property<int>("PedidoId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("ProdutoId")
                        .HasColumnType("INTEGER");

                    b.HasKey("PedidoId", "ProdutoId");

                    b.HasIndex("ProdutoId");

                    b.ToTable("PedidoItens");
                });

            modelBuilder.Entity("ApiPedidoVenda.Models.Pedido", b =>
                {
                    b.HasOne("ApiPedidoVenda.Models.Cliente", "Cliente")
                        .WithMany("Pedidos")
                        .HasForeignKey("ClienteId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .HasConstraintName("FK_Pedido_Cliente");

                    b.Navigation("Cliente");
                });

            modelBuilder.Entity("PedidoItens", b =>
                {
                    b.HasOne("ApiPedidoVenda.Models.Pedido", null)
                        .WithMany()
                        .HasForeignKey("PedidoId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired()
                        .HasConstraintName("FK_PedidoItens_PedidoId");

                    b.HasOne("ApiPedidoVenda.Models.Produto", null)
                        .WithMany()
                        .HasForeignKey("ProdutoId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired()
                        .HasConstraintName("FK_PedidoItens_ProdutoId");
                });

            modelBuilder.Entity("ApiPedidoVenda.Models.Cliente", b =>
                {
                    b.Navigation("Pedidos");
                });
#pragma warning restore 612, 618
        }
    }
}
