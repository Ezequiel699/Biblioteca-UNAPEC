using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Unapec.Biblioteca.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Add_Libros : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Autores",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Nombre = table.Column<string>(type: "varchar(120)", maxLength: 120, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PaisOrigen = table.Column<string>(type: "varchar(80)", maxLength: 80, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IdiomaNativoId = table.Column<int>(type: "int", nullable: false),
                    Estado = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: true),
                    CreadoEn = table.Column<DateTime>(type: "datetime(6)", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP(6)"),
                    ActualizadoEn = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Autores", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Autores_Idiomas_IdiomaNativoId",
                        column: x => x.IdiomaNativoId,
                        principalTable: "Idiomas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Libros",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Descripcion = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    SignaturaTopografica = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ISBN = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TipoBibliografiaId = table.Column<int>(type: "int", nullable: false),
                    EditoraId = table.Column<int>(type: "int", nullable: false),
                    AnioPublicacion = table.Column<int>(type: "int", nullable: false),
                    CienciaId = table.Column<int>(type: "int", nullable: false),
                    IdiomaId = table.Column<int>(type: "int", nullable: false),
                    Estado = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: true),
                    CreadoEn = table.Column<DateTime>(type: "datetime(6)", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP(6)"),
                    ActualizadoEn = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Libros", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Libros_Ciencias_CienciaId",
                        column: x => x.CienciaId,
                        principalTable: "Ciencias",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Libros_Editoras_EditoraId",
                        column: x => x.EditoraId,
                        principalTable: "Editoras",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Libros_Idiomas_IdiomaId",
                        column: x => x.IdiomaId,
                        principalTable: "Idiomas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Libros_TiposBibliografia_TipoBibliografiaId",
                        column: x => x.TipoBibliografiaId,
                        principalTable: "TiposBibliografia",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "AutoresLibros",
                columns: table => new
                {
                    AutorId = table.Column<int>(type: "int", nullable: false),
                    LibroId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AutoresLibros", x => new { x.AutorId, x.LibroId });
                    table.ForeignKey(
                        name: "FK_AutoresLibros_Autores_AutorId",
                        column: x => x.AutorId,
                        principalTable: "Autores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AutoresLibros_Libros_LibroId",
                        column: x => x.LibroId,
                        principalTable: "Libros",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Autores_IdiomaNativoId",
                table: "Autores",
                column: "IdiomaNativoId");

            migrationBuilder.CreateIndex(
                name: "IX_Autores_Nombre",
                table: "Autores",
                column: "Nombre");

            migrationBuilder.CreateIndex(
                name: "IX_AutoresLibros_LibroId",
                table: "AutoresLibros",
                column: "LibroId");

            migrationBuilder.CreateIndex(
                name: "IX_Libros_AnioPublicacion_IdiomaId",
                table: "Libros",
                columns: new[] { "AnioPublicacion", "IdiomaId" });

            migrationBuilder.CreateIndex(
                name: "IX_Libros_CienciaId",
                table: "Libros",
                column: "CienciaId");

            migrationBuilder.CreateIndex(
                name: "IX_Libros_Descripcion",
                table: "Libros",
                column: "Descripcion");

            migrationBuilder.CreateIndex(
                name: "IX_Libros_EditoraId",
                table: "Libros",
                column: "EditoraId");

            migrationBuilder.CreateIndex(
                name: "IX_Libros_IdiomaId",
                table: "Libros",
                column: "IdiomaId");

            migrationBuilder.CreateIndex(
                name: "IX_Libros_ISBN",
                table: "Libros",
                column: "ISBN");

            migrationBuilder.CreateIndex(
                name: "IX_Libros_TipoBibliografiaId",
                table: "Libros",
                column: "TipoBibliografiaId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AutoresLibros");

            migrationBuilder.DropTable(
                name: "Autores");

            migrationBuilder.DropTable(
                name: "Libros");
        }
    }
}
