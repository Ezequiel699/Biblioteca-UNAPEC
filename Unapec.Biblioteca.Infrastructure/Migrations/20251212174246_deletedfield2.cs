using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Unapec.Biblioteca.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class deletedfield2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AppUserId",
                table: "Usuarios",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AppUserId",
                table: "Empleados",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Rol",
                table: "AppUsers",
                type: "int",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(20)",
                oldMaxLength: 20)
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_AppUserId",
                table: "Usuarios",
                column: "AppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Empleados_AppUserId",
                table: "Empleados",
                column: "AppUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Empleados_AppUsers_AppUserId",
                table: "Empleados",
                column: "AppUserId",
                principalTable: "AppUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Usuarios_AppUsers_AppUserId",
                table: "Usuarios",
                column: "AppUserId",
                principalTable: "AppUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Empleados_AppUsers_AppUserId",
                table: "Empleados");

            migrationBuilder.DropForeignKey(
                name: "FK_Usuarios_AppUsers_AppUserId",
                table: "Usuarios");

            migrationBuilder.DropIndex(
                name: "IX_Usuarios_AppUserId",
                table: "Usuarios");

            migrationBuilder.DropIndex(
                name: "IX_Empleados_AppUserId",
                table: "Empleados");

            migrationBuilder.DropColumn(
                name: "AppUserId",
                table: "Usuarios");

            migrationBuilder.DropColumn(
                name: "AppUserId",
                table: "Empleados");

            migrationBuilder.AlterColumn<string>(
                name: "Rol",
                table: "AppUsers",
                type: "varchar(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldMaxLength: 20)
                .Annotation("MySql:CharSet", "utf8mb4");
        }
    }
}
