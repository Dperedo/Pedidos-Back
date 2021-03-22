using Microsoft.EntityFrameworkCore.Migrations;

namespace Pedidos_back.Migrations
{
    public partial class Ingresar : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var sqlline = $" IF (SELECT COUNT(*) FROM Clientes) = 0 " +
            $" BEGIN " +
            $" INSERT [dbo].[Clientes] ([Id], [RUT], [RazonSocial], [FechaDeCreacion], [Vigente]) VALUES (N'511d6c76-8e80-4fa4-cc03-08d866511f3d', N'18.333.111.9', N'Javier', CAST(N'2020-10-01T21:29:47.3400038' AS DateTime2), 0) " +
            $" INSERT [dbo].[Clientes] ([Id], [RUT], [RazonSocial], [FechaDeCreacion], [Vigente]) VALUES (N'125ee3ce-6f86-4a1f-cc04-08d866511f3d', N'183331116', N'Diego', CAST(N'2020-10-01T21:30:13.9438064' AS DateTime2), 1) " +
            $" END";
            migrationBuilder.Sql(sqlline);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM Clientes");
        }
    }
}
