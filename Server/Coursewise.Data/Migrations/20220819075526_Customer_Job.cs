using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Coursewise.Data.Migrations
{
    public partial class Customer_Job : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CustomerJobs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Email = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Description = table.Column<string>(type: "varchar(1500)", maxLength: 1500, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ApplyAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    ArtifactId = table.Column<int>(type: "int", nullable: true),
                    CustomerId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    JobId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerJobs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CustomerJobs_Artifacts_ArtifactId",
                        column: x => x.ArtifactId,
                        principalTable: "Artifacts",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CustomerJobs_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CustomerJobs_Jobs_JobId",
                        column: x => x.JobId,
                        principalTable: "Jobs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerJobs_ArtifactId",
                table: "CustomerJobs",
                column: "ArtifactId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerJobs_CustomerId",
                table: "CustomerJobs",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerJobs_JobId",
                table: "CustomerJobs",
                column: "JobId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CustomerJobs");
        }
    }
}
