using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Portal.Infra.Data.Repository.Migrations
{
    /// <inheritdoc />
    public partial class RetiradoDataAnnotation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Limpeza para Vendedores (CPF - 11)
            migrationBuilder.Sql("UPDATE Vendedores SET Cpf = REPLACE(REPLACE(REPLACE(Cpf, '.', ''), '-', ''), ' ', '') WHERE LEN(Cpf) > 11;");

            // Limpeza para Invoices (Pode ser CPF ou CNPJ - até 14)
            migrationBuilder.Sql("UPDATE Invoices SET ClienteDocumento = REPLACE(REPLACE(REPLACE(REPLACE(ClienteDocumento, '.', ''), '-', ''), '/', ''), ' ', '') WHERE LEN(ClienteDocumento) > 14;");

            migrationBuilder.AlterColumn<string>(
                name: "Cpf",
                table: "Vendedores",
                type: "nchar(11)",
                fixedLength: true,
                maxLength: 11,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "ClienteDocumento",
                table: "Invoices",
                type: "nvarchar(14)",
                maxLength: 14,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Cpf",
                table: "Vendedores",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nchar(11)",
                oldFixedLength: true,
                oldMaxLength: 11);

            migrationBuilder.AlterColumn<string>(
                name: "ClienteDocumento",
                table: "Invoices",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(14)",
                oldMaxLength: 14);
        }
    }
}
