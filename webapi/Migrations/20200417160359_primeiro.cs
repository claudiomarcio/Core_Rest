using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace webapi.Migrations
{
    public partial class primeiro : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GrupoEmpresa",
                columns: table => new
                {
                    GrupoEmpresaId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    MatrizIdExterno = table.Column<int>(nullable: false),
                    EmpresaId = table.Column<int>(nullable: false),
                    SistemaId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GrupoEmpresa", x => x.GrupoEmpresaId);
                });

            migrationBuilder.CreateTable(
                name: "Menus_Acesso",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Descricao_Menu = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Menus_Acesso", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Perfil",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Descricao = table.Column<string>(nullable: true),
                    IDs_Menus_Acesso = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Perfil", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Sistema",
                columns: table => new
                {
                    SistemaId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Nome = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sistema", x => x.SistemaId);
                });

            migrationBuilder.CreateTable(
                name: "Empresa",
                columns: table => new
                {
                    EmpresaId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Nome = table.Column<string>(maxLength: 100, nullable: true),
                    IdExterno = table.Column<int>(nullable: false),
                    Ativo = table.Column<bool>(nullable: false),
                    SistemaId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Empresa", x => x.EmpresaId);
                    table.ForeignKey(
                        name: "FK_Empresa_Sistema_SistemaId",
                        column: x => x.SistemaId,
                        principalTable: "Sistema",
                        principalColumn: "SistemaId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Usuario",
                columns: table => new
                {
                    UsuarioId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Nome = table.Column<string>(maxLength: 100, nullable: true),
                    Senha = table.Column<string>(nullable: true),
                    NovaSenha = table.Column<string>(nullable: true),
                    Email = table.Column<string>(maxLength: 100, nullable: true),
                    Token = table.Column<string>(nullable: true),
                    DataUltimoLogin = table.Column<DateTime>(nullable: false),
                    DataCriacao = table.Column<DateTime>(nullable: false),
                    DataAtualizacao = table.Column<DateTime>(nullable: false),
                    EmpresaId = table.Column<int>(nullable: false),
                    PerfilID = table.Column<int>(nullable: false),
                    ativo = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuario", x => x.UsuarioId);
                    table.ForeignKey(
                        name: "FK_Usuario_Empresa_EmpresaId",
                        column: x => x.EmpresaId,
                        principalTable: "Empresa",
                        principalColumn: "EmpresaId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Usuario_Perfil_PerfilID",
                        column: x => x.PerfilID,
                        principalTable: "Perfil",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Empresa_IdExterno",
                table: "Empresa",
                column: "IdExterno");

            migrationBuilder.CreateIndex(
                name: "IX_Empresa_SistemaId",
                table: "Empresa",
                column: "SistemaId");

            migrationBuilder.CreateIndex(
                name: "IX_Usuario_EmpresaId",
                table: "Usuario",
                column: "EmpresaId");

            migrationBuilder.CreateIndex(
                name: "IX_Usuario_PerfilID",
                table: "Usuario",
                column: "PerfilID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GrupoEmpresa");

            migrationBuilder.DropTable(
                name: "Menus_Acesso");

            migrationBuilder.DropTable(
                name: "Usuario");

            migrationBuilder.DropTable(
                name: "Empresa");

            migrationBuilder.DropTable(
                name: "Perfil");

            migrationBuilder.DropTable(
                name: "Sistema");
        }
    }
}
