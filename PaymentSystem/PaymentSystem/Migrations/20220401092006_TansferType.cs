using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PaymentSystem.Migrations
{
    public partial class TansferType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "transfer_type",
                table: "fund_transfers",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: 2,
                column: "registered_at",
                value: new DateTime(2022, 4, 1, 9, 20, 5, 827, DateTimeKind.Utc).AddTicks(5827));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "transfer_type",
                table: "fund_transfers");

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: 2,
                column: "registered_at",
                value: new DateTime(2022, 3, 31, 13, 7, 44, 685, DateTimeKind.Utc).AddTicks(9272));
        }
    }
}
