using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace PaymentSystem.Migrations
{
    public partial class VerificationTransfers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_balances",
                table: "balances");

            migrationBuilder.DeleteData(
                table: "balances",
                keyColumn: "user_id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "users",
                keyColumn: "id",
                keyValue: 1);

            migrationBuilder.DropColumn(
                name: "Passport_data",
                table: "users");

            migrationBuilder.AlterColumn<bool>(
                name: "is_verified",
                table: "users",
                type: "boolean",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "id",
                table: "balances",
                type: "integer",
                nullable: false,
                defaultValue: 0)
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddPrimaryKey(
                name: "PK_balances",
                table: "balances",
                column: "id");

            migrationBuilder.CreateTable(
                name: "verification_transfers",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_id = table.Column<int>(type: "integer", nullable: false),
                    confirmed_by = table.Column<int>(type: "integer", nullable: true),
                    Passport_data = table.Column<string>(type: "text", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    confirmed_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_verification_transfers", x => x.id);
                    table.ForeignKey(
                        name: "FK_verification_transfers_users_confirmed_by",
                        column: x => x.confirmed_by,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_verification_transfers_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "users",
                columns: new[] { "id", "email", "first_name", "is_blocked", "is_verified", "last_name", "password", "registered_at" },
                values: new object[] { 2, "admin@gmail.com", "admin", false, false, "admin", "admin1234", new DateTime(2022, 3, 29, 10, 17, 56, 249, DateTimeKind.Utc).AddTicks(9794) });

            migrationBuilder.InsertData(
                table: "balances",
                columns: new[] { "id", "amount", "user_id" },
                values: new object[] { 1, 1000m, 2 });

            migrationBuilder.UpdateData(
                table: "user_roles",
                keyColumn: "id",
                keyValue: 1,
                column: "user_id",
                value: 2);

            migrationBuilder.CreateIndex(
                name: "IX_balances_user_id",
                table: "balances",
                column: "user_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_verification_transfers_confirmed_by",
                table: "verification_transfers",
                column: "confirmed_by");

            migrationBuilder.CreateIndex(
                name: "IX_verification_transfers_user_id",
                table: "verification_transfers",
                column: "user_id",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "verification_transfers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_balances",
                table: "balances");

            migrationBuilder.DropIndex(
                name: "IX_balances_user_id",
                table: "balances");

            migrationBuilder.DeleteData(
                table: "balances",
                keyColumn: "id",
                keyColumnType: "integer",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "users",
                keyColumn: "id",
                keyValue: 2);

            migrationBuilder.DropColumn(
                name: "id",
                table: "balances");

            migrationBuilder.AlterColumn<bool>(
                name: "is_verified",
                table: "users",
                type: "boolean",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "boolean");

            migrationBuilder.AddColumn<string>(
                name: "Passport_data",
                table: "users",
                type: "text",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_balances",
                table: "balances",
                column: "user_id");

            migrationBuilder.InsertData(
                table: "users",
                columns: new[] { "id", "email", "first_name", "is_blocked", "is_verified", "last_name", "Passport_data", "password", "registered_at" },
                values: new object[] { 1, "admin@gmail.com", "admin", false, true, "admin", null, "admin1234", new DateTime(2022, 3, 22, 9, 44, 33, 120, DateTimeKind.Utc).AddTicks(3923) });

            migrationBuilder.InsertData(
                table: "balances",
                columns: new[] { "user_id", "amount" },
                values: new object[] { 1, 1000m });

            migrationBuilder.UpdateData(
                table: "user_roles",
                keyColumn: "id",
                keyValue: 1,
                column: "user_id",
                value: 1);
        }
    }
}
