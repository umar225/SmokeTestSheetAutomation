using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Coursewise.Data.Migrations
{
    public partial class membership_fields_added : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "MembershipExpiryDate",
                table: "Customers",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "isMember",
                table: "Customers",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MembershipExpiryDate",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "isMember",
                table: "Customers");
        }
    }
}
