using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Coursewise.Data.Migrations
{
    public partial class Job_Notification_Check : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsEmailSent",
                table: "Jobs",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);
            migrationBuilder.Sql($"UPDATE jobs SET IsEmailSent=1;");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsEmailSent",
                table: "Jobs");
        }
    }
}
