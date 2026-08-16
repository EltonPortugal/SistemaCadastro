using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CadastroFuncionarios.Migrations
{
    /// <inheritdoc />
    public partial class InicialTPH : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Funcionarios",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nome = table.Column<string>(type: "TEXT", maxLength: 150, nullable: false),
                    Email = table.Column<string>(type: "TEXT", nullable: false),
                    DataContratacao = table.Column<DateTime>(type: "TEXT", nullable: false),
                    SalarioBase = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TipoFuncionario = table.Column<string>(type: "TEXT", maxLength: 13, nullable: false),
                    NivelSenioridade = table.Column<int>(type: "INTEGER", nullable: true),
                    CertificacoesQtd = table.Column<int>(type: "INTEGER", nullable: true),
                    Instituicao = table.Column<string>(type: "TEXT", maxLength: 150, nullable: true),
                    AuxilioTransporte = table.Column<decimal>(type: "TEXT", nullable: true),
                    Bonus = table.Column<decimal>(type: "TEXT", nullable: true),
                    TamanhoEquipe = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Funcionarios", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Funcionarios");
        }
    }
}
