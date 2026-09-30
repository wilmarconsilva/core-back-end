using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Shared.Persistance.Migrations
{
    /// <inheritdoc />
    public partial class dev_50 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "payment_installment_has_installment",
                table: "orders",
                newName: "payment_has_installment");

            migrationBuilder.RenameColumn(
                name: "payment_installment_has_installment",
                table: "budgets",
                newName: "payment_has_installment");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "payment_has_installment",
                table: "orders",
                newName: "payment_installment_has_installment");

            migrationBuilder.RenameColumn(
                name: "payment_has_installment",
                table: "budgets",
                newName: "payment_installment_has_installment");
        }
    }
}
