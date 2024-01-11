using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Coursewise.Data.Migrations
{
    public partial class isEmailSent : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsEmailSent",
                table: "Resources",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "isDeleted",
                table: "Resources",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsEmailSent",
                table: "Resources");

            migrationBuilder.DropColumn(
                name: "isDeleted",
                table: "Resources");
        }
    }
}
