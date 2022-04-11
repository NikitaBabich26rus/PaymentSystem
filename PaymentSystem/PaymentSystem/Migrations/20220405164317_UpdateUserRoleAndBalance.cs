using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace PaymentSystem.Migrations
{
    public partial class UpdateUserRoleAndBalance : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "verification_transfers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_user_roles",
                table: "user_roles");

            migrationBuilder.DropIndex(
                name: "IX_user_roles_user_id",
                table: "user_roles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_balances",
                table: "balances");

            migrationBuilder.DropIndex(
                name: "IX_balances_user_id",
                table: "balances");

            migrationBuilder.DropColumn(
                name: "id",
                table: "user_roles");

            migrationBuilder.DropColumn(
                name: "id",
                table: "balances");

            migrationBuilder.AddPrimaryKey(
                name: "PK_user_roles",
                table: "user_roles",
                column: "user_id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_balances",
                table: "balances",
                column: "user_id");

            migrationBuilder.CreateTable(
                name: "verifications",
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
                    table.PrimaryKey("PK_verifications", x => x.id);
                    table.ForeignKey(
                        name: "FK_verifications_users_confirmed_by",
                        column: x => x.confirmed_by,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_verifications_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: 2,
                column: "registered_at",
                value: new DateTime(2022, 4, 5, 16, 43, 17, 391, DateTimeKind.Utc).AddTicks(478));

            migrationBuilder.CreateIndex(
                name: "IX_verifications_confirmed_by",
                table: "verifications",
                column: "confirmed_by");

            migrationBuilder.CreateIndex(
                name: "IX_verifications_user_id",
                table: "verifications",
                column: "user_id",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "verifications");

            migrationBuilder.DropPrimaryKey(
                name: "PK_user_roles",
                table: "user_roles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_balances",
                table: "balances");

            migrationBuilder.DeleteData(
                table: "balances",
                keyColumn: "user_id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "user_roles",
                keyColumn: "user_id",
                keyValue: 2);

            migrationBuilder.AddColumn<int>(
                name: "id",
                table: "user_roles",
                type: "integer",
                nullable: false,
                defaultValue: 0)
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddColumn<int>(
                name: "id",
                table: "balances",
                type: "integer",
                nullable: false,
                defaultValue: 0)
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddPrimaryKey(
                name: "PK_user_roles",
                table: "user_roles",
                column: "id");

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
                    confirmed_by = table.Column<int>(type: "integer", nullable: true),
                    user_id = table.Column<int>(type: "integer", nullable: false),
                    confirmed_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Passport_data = table.Column<string>(type: "text", nullable: false)
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
                table: "balances",
                columns: new[] { "id", "amount", "user_id" },
                values: new object[] { 1, 1000m, 2 });

            migrationBuilder.InsertData(
                table: "user_roles",
                columns: new[] { "id", "role_id", "user_id" },
                values: new object[] { 1, 2, 2 });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: 2,
                column: "registered_at",
                value: new DateTime(2022, 4, 1, 9, 20, 5, 827, DateTimeKind.Utc).AddTicks(5827));

            migrationBuilder.CreateIndex(
                name: "IX_user_roles_user_id",
                table: "user_roles",
                column: "user_id");

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
    }
}
