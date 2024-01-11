using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Coursewise.Data.Migrations
{
    public partial class Job_Title : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Titles",
                columns: table => new
                {
                    TitleId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    NoOfJob = table.Column<int>(type: "int", nullable: false, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Titles", x => x.TitleId);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "JobTitles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Other = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    JobId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    TitleId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobTitles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_JobTitles_Jobs_JobId",
                        column: x => x.JobId,
                        principalTable: "Jobs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_JobTitles_Titles_TitleId",
                        column: x => x.TitleId,
                        principalTable: "Titles",
                        principalColumn: "TitleId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_JobTitles_JobId",
                table: "JobTitles",
                column: "JobId");

            migrationBuilder.CreateIndex(
                name: "IX_JobTitles_TitleId",
                table: "JobTitles",
                column: "TitleId");

            migrationBuilder.Sql($"INSERT INTO `titles` (`Name`) VALUES ('Non-Executive Director');");
            migrationBuilder.Sql($"INSERT INTO `titles` (`Name`) VALUES ('Board Advisor');");

            migrationBuilder.Sql($"CREATE TRIGGER `JobTitles_AFTER_INSERT` AFTER INSERT ON `JobTitles` FOR EACH ROW BEGIN update Titles set NoOfJob=NoOfJob+1 where TitleId = new.TitleId; END");
            migrationBuilder.Sql($"CREATE TRIGGER `JobTitles_AFTER_DELETE` AFTER DELETE ON `JobTitles` FOR EACH ROW BEGIN update Titles set NoOfJob=NoOfJob-1 where TitleId = old.TitleId; END");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql($"DROP TRIGGER `JobTitles_AFTER_INSERT`");
            migrationBuilder.Sql($"DROP TRIGGER `JobTitles_AFTER_DELETE`");

            migrationBuilder.DropTable(
                name: "JobTitles");

            migrationBuilder.DropTable(
                name: "Titles");
        }
    }
}
