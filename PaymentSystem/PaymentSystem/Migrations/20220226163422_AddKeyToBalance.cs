using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PaymentSystem.Migrations
{
    public partial class AddKeyToBalance : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_balances_user_id",
                table: "balances");

            migrationBuilder.AddPrimaryKey(
                name: "PK_balances",
                table: "balances",
                column: "user_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_balances",
                table: "balances");

            migrationBuilder.CreateIndex(
                name: "IX_balances_user_id",
                table: "balances",
                column: "user_id",
                unique: true);
        }
    }
}
