using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Shared.Persistance.Migrations
{
    /// <inheritdoc />
    public partial class dev_28 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_budgets_users_user_id",
                table: "budgets");

            migrationBuilder.DropForeignKey(
                name: "FK_orders_users_user_id",
                table: "orders");

            migrationBuilder.RenameColumn(
                name: "user_id",
                table: "orders",
                newName: "seller_id");

            migrationBuilder.RenameIndex(
                name: "IX_orders_user_id",
                table: "orders",
                newName: "IX_orders_seller_id");

            migrationBuilder.RenameColumn(
                name: "user_id",
                table: "budgets",
                newName: "seller_id");

            migrationBuilder.RenameIndex(
                name: "IX_budgets_user_id",
                table: "budgets",
                newName: "IX_budgets_seller_id");

            migrationBuilder.AddForeignKey(
                name: "FK_budgets_users_seller_id",
                table: "budgets",
                column: "seller_id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_orders_users_seller_id",
                table: "orders",
                column: "seller_id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_budgets_users_seller_id",
                table: "budgets");

            migrationBuilder.DropForeignKey(
                name: "FK_orders_users_seller_id",
                table: "orders");

            migrationBuilder.RenameColumn(
                name: "seller_id",
                table: "orders",
                newName: "user_id");

            migrationBuilder.RenameIndex(
                name: "IX_orders_seller_id",
                table: "orders",
                newName: "IX_orders_user_id");

            migrationBuilder.RenameColumn(
                name: "seller_id",
                table: "budgets",
                newName: "user_id");

            migrationBuilder.RenameIndex(
                name: "IX_budgets_seller_id",
                table: "budgets",
                newName: "IX_budgets_user_id");

            migrationBuilder.AddForeignKey(
                name: "FK_budgets_users_user_id",
                table: "budgets",
                column: "user_id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_orders_users_user_id",
                table: "orders",
                column: "user_id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
