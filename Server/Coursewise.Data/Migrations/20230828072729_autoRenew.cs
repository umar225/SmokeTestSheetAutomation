using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Coursewise.Data.Migrations
{
    public partial class autoRenew : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "AutoRenewMembership",
                table: "Customers",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AutoRenewMembership",
                table: "Customers");
        }
    }
}
