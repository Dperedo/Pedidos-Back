using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Pedidos_back.Migrations
{
    public partial class Proyecto : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Clientes",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    RUT = table.Column<string>(maxLength: 20, nullable: true),
                    RazonSocial = table.Column<string>(maxLength: 20, nullable: true),
                    FechaDeCreacion = table.Column<DateTime>(nullable: false),
                    Vigente = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clientes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Estados",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    EstadoPedido = table.Column<string>(maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Estados", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Productos",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Codigo = table.Column<string>(maxLength: 20, nullable: true),
                    Nombre = table.Column<string>(maxLength: 200, nullable: true),
                    Descripcion = table.Column<string>(nullable: true),
                    Precio = table.Column<float>(nullable: false),
                    FechaDeCreacion = table.Column<DateTime>(nullable: false),
                    Vigente = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Productos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Usuarios",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Username = table.Column<string>(maxLength: 20, nullable: true),
                    Nombre = table.Column<string>(maxLength: 20, nullable: true),
                    RUT = table.Column<string>(maxLength: 20, nullable: true),
                    PasswordHash = table.Column<byte[]>(nullable: true),
                    PasswordSalt = table.Column<byte[]>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuarios", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Clientes");

            migrationBuilder.DropTable(
                name: "Estados");

            migrationBuilder.DropTable(
                name: "Productos");

            migrationBuilder.DropTable(
                name: "Usuarios");
        }
    }
}
