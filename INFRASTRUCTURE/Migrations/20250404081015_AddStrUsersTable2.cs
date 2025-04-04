using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace INFRASTRUCTURE.Migrations
{
    /// <inheritdoc />
    public partial class AddStrUsersTable2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            

            migrationBuilder.CreateTable(
                name: "tbl_users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    str_user_name = table.Column<string>(type: "text", nullable: false),
                    str_password = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_users", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tbl_forms_str_form_id",
                table: "tbl_forms",
                column: "str_form_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_tbl_refmas_accounts_str_name",
                table: "tbl_refmas_accounts",
                column: "str_name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_tbl_refmas_expense_str_name",
                table: "tbl_refmas_expense",
                column: "str_name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_tbl_refmas_income_str_name",
                table: "tbl_refmas_income",
                column: "str_name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_tbl_trns_str_id",
                table: "tbl_trns",
                column: "str_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_tbl_trns_records_str_id",
                table: "tbl_trns_records",
                column: "str_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_tbl_users_str_user_name",
                table: "tbl_users",
                column: "str_user_name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tbl_forms");

            migrationBuilder.DropTable(
                name: "tbl_refmas_accounts");

            migrationBuilder.DropTable(
                name: "tbl_refmas_expense");

            migrationBuilder.DropTable(
                name: "tbl_refmas_income");

            migrationBuilder.DropTable(
                name: "tbl_trns");

            migrationBuilder.DropTable(
                name: "tbl_trns_records");

            migrationBuilder.DropTable(
                name: "tbl_users");
        }
    }
}
