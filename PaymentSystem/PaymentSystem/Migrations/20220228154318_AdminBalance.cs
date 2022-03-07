using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PaymentSystem.Migrations
{
    public partial class AdminBalance : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "balances",
                columns: new[] { "user_id", "amount" },
                values: new object[] { 1, 1000m });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: 1,
                column: "registered_at",
                value: new DateTime(2022, 2, 28, 15, 43, 18, 504, DateTimeKind.Utc).AddTicks(7881));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "balances",
                keyColumn: "user_id",
                keyValue: 1);

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: 1,
                column: "registered_at",
                value: new DateTime(2022, 2, 28, 15, 35, 20, 631, DateTimeKind.Utc).AddTicks(8795));
        }
    }
}
