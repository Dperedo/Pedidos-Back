using Microsoft.EntityFrameworkCore.Migrations;

namespace Pedidos_back.Migrations
{
    public partial class UsuarioTest : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {

                byte[] passwordHash, passwordSalt;
                Repository.UserService.CreatePasswordHash("1234", out passwordHash, out passwordSalt);

                migrationBuilder.InsertData(
                table: "Usuarios",
                columns: new[] { "Id", "Username", "Nombre","RUT", "PasswordHash","PasswordSalt" },
                values: new object[] { "150108E0-8EF5-470A-232C-08D8CD1BD7A6", "test@test.com",
                "","",
                passwordHash,
                passwordSalt });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Usuarios", 
                keyColumn: "Id", 
                keyValue: "150108E0-8EF5-470A-232C-08D8CD1BD7A6");
        }
    }
}
