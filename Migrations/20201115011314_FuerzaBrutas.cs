using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Pedidos_back.Migrations
{
    public partial class FuerzaBrutas : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            /*
            migrationBuilder.AlterColumn<long>(
                name: "Secuencial",
                table: "Pedidos",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint")
                .Annotation("SqlServer:Identity", "1, 1");
*/
            migrationBuilder.InsertData(
                table: "Estados",
                columns: new[] { "Id", "EstadoPedido" },
                values: new object[] { Guid.NewGuid(), "Nuevo" });

            migrationBuilder.InsertData(
                table: "Estados",
                columns: new[] { "Id", "EstadoPedido" },
                values: new object[] { Guid.NewGuid(), "Cancelado" });

            migrationBuilder.InsertData(
                table: "Estados",
                columns: new[] { "Id", "EstadoPedido" },
                values: new object[] { Guid.NewGuid(), "Completado" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

            /*
            migrationBuilder.AlterColumn<long>(
                name: "Secuencial",
                table: "Pedidos",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long))
                .OldAnnotation("SqlServer:Identity", "1, 1");
*/
            migrationBuilder.DeleteData(
                table: "Estados", 
                keyColumn: "EstadoPedido", 
                keyValue: "Nuevo");
            migrationBuilder.DeleteData(
                table: "Estados", 
                keyColumn: "EstadoPedido", 
                keyValue: "Cancelado");                
            migrationBuilder.DeleteData(
                table: "Estados", 
                keyColumn: "EstadoPedido", 
                keyValue: "Completado"); 
        }
    }
}
