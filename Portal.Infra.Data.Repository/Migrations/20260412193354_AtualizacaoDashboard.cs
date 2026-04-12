using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Portal.Infra.Data.Repository.Migrations
{
    /// <inheritdoc />
    public partial class AtualizacaoDashboard : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Invoices_Vendedores_VendedorId",
                table: "Invoices");

            migrationBuilder.DropIndex(
                name: "IX_Comissoes_InvoiceId",
                table: "Comissoes");

            migrationBuilder.RenameColumn(
                name: "IssueDate",
                table: "Invoices",
                newName: "DataEmissao");

            migrationBuilder.AlterColumn<string>(
                name: "Cpf",
                table: "Vendedores",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_Vendedores_Cpf",
                table: "Vendedores",
                column: "Cpf",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Vendedores_Email",
                table: "Vendedores",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Invoices_Number",
                table: "Invoices",
                column: "Number",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Comissoes_InvoiceId",
                table: "Comissoes",
                column: "InvoiceId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Invoices_Vendedores_VendedorId",
                table: "Invoices",
                column: "VendedorId",
                principalTable: "Vendedores",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Invoices_Vendedores_VendedorId",
                table: "Invoices");

            migrationBuilder.DropIndex(
                name: "IX_Vendedores_Cpf",
                table: "Vendedores");

            migrationBuilder.DropIndex(
                name: "IX_Vendedores_Email",
                table: "Vendedores");

            migrationBuilder.DropIndex(
                name: "IX_Invoices_Number",
                table: "Invoices");

            migrationBuilder.DropIndex(
                name: "IX_Comissoes_InvoiceId",
                table: "Comissoes");

            migrationBuilder.RenameColumn(
                name: "DataEmissao",
                table: "Invoices",
                newName: "IssueDate");

            migrationBuilder.AlterColumn<string>(
                name: "Cpf",
                table: "Vendedores",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.CreateIndex(
                name: "IX_Comissoes_InvoiceId",
                table: "Comissoes",
                column: "InvoiceId");

            migrationBuilder.AddForeignKey(
                name: "FK_Invoices_Vendedores_VendedorId",
                table: "Invoices",
                column: "VendedorId",
                principalTable: "Vendedores",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
