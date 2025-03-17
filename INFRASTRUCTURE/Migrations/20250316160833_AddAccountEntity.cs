using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace INFRASTRUCTURE.Migrations
{
    /// <inheritdoc />
    public partial class AddAccountEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tbl_refmas_accounts",
                columns: table => new
                {
                    str_id = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    str_name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    str_active = table.Column<string>(type: "character varying(1)", maxLength: 1, nullable: false),
                    str_user = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    dtm_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    str_ismain = table.Column<char>(type: "character(1)", maxLength: 1, nullable: false, defaultValue: 'N')
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_refmas_accounts", x => x.str_id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tbl_refmas_accounts_str_name",
                table: "tbl_refmas_accounts",
                column: "str_name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tbl_refmas_accounts");
        }
    }
}
