using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PaymentSystem.Migrations
{
    public partial class AmountOfMoneyInFundTransfer : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "amount_of_money",
                table: "fund_transfers",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: 2,
                column: "registered_at",
                value: new DateTime(2022, 3, 31, 13, 7, 44, 685, DateTimeKind.Utc).AddTicks(9272));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "amount_of_money",
                table: "fund_transfers");

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: 2,
                column: "registered_at",
                value: new DateTime(2022, 3, 29, 10, 17, 56, 249, DateTimeKind.Utc).AddTicks(9794));
        }
    }
}
