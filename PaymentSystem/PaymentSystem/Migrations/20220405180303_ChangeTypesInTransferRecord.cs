using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PaymentSystem.Migrations
{
    public partial class ChangeTypesInTransferRecord : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "user_date",
                table: "fund_transfers",
                newName: "card_date");
            
            migrationBuilder.Sql("alter table payment_system.public.fund_transfers alter column card_number TYPE bigint USING (card_number::bigint);");

            migrationBuilder.Sql("alter table payment_system.public.fund_transfers alter column card_cvc TYPE integer USING (card_number::integer);");

            migrationBuilder.Sql("alter table payment_system.public.fund_transfers alter column card_date TYPE integer USING (card_number::integer);");

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: 2,
                column: "registered_at",
                value: new DateTime(2022, 4, 5, 18, 3, 3, 239, DateTimeKind.Utc).AddTicks(5807));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "card_date",
                table: "fund_transfers",
                newName: "user_date");

            migrationBuilder.AlterColumn<string>(
                name: "card_number",
                table: "fund_transfers",
                type: "text",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<string>(
                name: "card_cvc",
                table: "fund_transfers",
                type: "text",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<string>(
                name: "user_date",
                table: "fund_transfers",
                type: "text",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: 2,
                column: "registered_at",
                value: new DateTime(2022, 4, 5, 16, 43, 17, 391, DateTimeKind.Utc).AddTicks(478));
        }
    }
}
