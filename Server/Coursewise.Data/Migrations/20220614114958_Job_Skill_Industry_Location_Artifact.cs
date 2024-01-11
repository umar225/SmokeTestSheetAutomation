using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Coursewise.Data.Migrations
{
    public partial class Job_Skill_Industry_Location_Artifact : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Artifacts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    StorageLocation = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FileType = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Url = table.Column<string>(type: "varchar(512)", maxLength: 512, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Artifacts", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Industries",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Industries", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Jobs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Name = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Url = table.Column<string>(type: "varchar(600)", maxLength: 600, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Company = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CompanyTagLine = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CompanyLink = table.Column<string>(type: "varchar(600)", maxLength: 600, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    NoOfRole = table.Column<int>(type: "int", nullable: false),
                    ShortDescription = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Description = table.Column<string>(type: "longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Category = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Created = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Expire = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    IsVisible = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    IsClosed = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Jobs", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Locations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Locations", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Skills",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Skills", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "JobArtifacts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Type = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    JobId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    ArtifactId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobArtifacts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_JobArtifacts_Artifacts_ArtifactId",
                        column: x => x.ArtifactId,
                        principalTable: "Artifacts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_JobArtifacts_Jobs_JobId",
                        column: x => x.JobId,
                        principalTable: "Jobs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "JobIndustries",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Other = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    JobId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    IndustryId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobIndustries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_JobIndustries_Industries_IndustryId",
                        column: x => x.IndustryId,
                        principalTable: "Industries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_JobIndustries_Jobs_JobId",
                        column: x => x.JobId,
                        principalTable: "Jobs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "JobLocations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    JobId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Other = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    LocationId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobLocations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_JobLocations_Jobs_JobId",
                        column: x => x.JobId,
                        principalTable: "Jobs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_JobLocations_Locations_LocationId",
                        column: x => x.LocationId,
                        principalTable: "Locations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "JobSkills",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Other = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    JobId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    SkillId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobSkills", x => x.Id);
                    table.ForeignKey(
                        name: "FK_JobSkills_Jobs_JobId",
                        column: x => x.JobId,
                        principalTable: "Jobs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_JobSkills_Skills_SkillId",
                        column: x => x.SkillId,
                        principalTable: "Skills",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_JobArtifacts_ArtifactId",
                table: "JobArtifacts",
                column: "ArtifactId");

            migrationBuilder.CreateIndex(
                name: "IX_JobArtifacts_JobId",
                table: "JobArtifacts",
                column: "JobId");

            migrationBuilder.CreateIndex(
                name: "IX_JobIndustries_IndustryId",
                table: "JobIndustries",
                column: "IndustryId");

            migrationBuilder.CreateIndex(
                name: "IX_JobIndustries_JobId",
                table: "JobIndustries",
                column: "JobId");

            migrationBuilder.CreateIndex(
                name: "IX_JobLocations_JobId",
                table: "JobLocations",
                column: "JobId");

            migrationBuilder.CreateIndex(
                name: "IX_JobLocations_LocationId",
                table: "JobLocations",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_JobSkills_JobId",
                table: "JobSkills",
                column: "JobId");

            migrationBuilder.CreateIndex(
                name: "IX_JobSkills_SkillId",
                table: "JobSkills",
                column: "SkillId");

            migrationBuilder.Sql($"INSERT INTO `industries` (`Name`) VALUES ('Accommodation and Food Service Activities');");
            migrationBuilder.Sql($"INSERT INTO `industries` (`Name`) VALUES ('Administrative and Support Service Activities');");
            migrationBuilder.Sql($"INSERT INTO `industries` (`Name`) VALUES ('Arts, Entertainment and Recreation');");
            migrationBuilder.Sql($"INSERT INTO `industries` (`Name`) VALUES ('Construction');");
            migrationBuilder.Sql($"INSERT INTO `industries` (`Name`) VALUES ('Education');");
            migrationBuilder.Sql($"INSERT INTO `industries` (`Name`) VALUES ('Electricity, Gas, Steam and Air Conditioning Supply');");
            migrationBuilder.Sql($"INSERT INTO `industries` (`Name`) VALUES ('Financial and Insurance Activities');");
            migrationBuilder.Sql($"INSERT INTO `industries` (`Name`) VALUES ('Human Health and Social Work Activities');");
            migrationBuilder.Sql($"INSERT INTO `industries` (`Name`) VALUES ('Information and Communication');");
            migrationBuilder.Sql($"INSERT INTO `industries` (`Name`) VALUES ('Manufacturing');");
            migrationBuilder.Sql($"INSERT INTO `industries` (`Name`) VALUES ('Mining and Quarrying');");
            migrationBuilder.Sql($"INSERT INTO `industries` (`Name`) VALUES ('Other Service Activities');");
            migrationBuilder.Sql($"INSERT INTO `industries` (`Name`) VALUES ('Professional, Scientific and Technical Activities');");
            migrationBuilder.Sql($"INSERT INTO `industries` (`Name`) VALUES ('Real Estate Activities');");
            migrationBuilder.Sql($"INSERT INTO `industries` (`Name`) VALUES ('Transportation and Storage');");
            migrationBuilder.Sql($"INSERT INTO `industries` (`Name`) VALUES ('Water Supply; Sewerage, Waste Management and Remediation Activities');");
            migrationBuilder.Sql($"INSERT INTO `industries` (`Name`) VALUES ('Wholesale and Retail Trade');");
            migrationBuilder.Sql($"INSERT INTO `industries` (`Name`) VALUES ('Consultancy');");
            migrationBuilder.Sql($"INSERT INTO `industries` (`Name`) VALUES ('Other');");

            migrationBuilder.Sql($"INSERT INTO `skills` (`Name`) VALUES ('Good Governance');");
            migrationBuilder.Sql($"INSERT INTO `skills` (`Name`) VALUES ('Board-level experience');");
            migrationBuilder.Sql($"INSERT INTO `skills` (`Name`) VALUES ('Commercial Delivery');");
            migrationBuilder.Sql($"INSERT INTO `skills` (`Name`) VALUES ('Accounting');");
            migrationBuilder.Sql($"INSERT INTO `skills` (`Name`) VALUES ('Digital');");
            migrationBuilder.Sql($"INSERT INTO `skills` (`Name`) VALUES ('Partnerships');");
            migrationBuilder.Sql($"INSERT INTO `skills` (`Name`) VALUES ('Legal');");
            migrationBuilder.Sql($"INSERT INTO `skills` (`Name`) VALUES ('People Management');");
            migrationBuilder.Sql($"INSERT INTO `skills` (`Name`) VALUES ('Workforce Planning');");
            migrationBuilder.Sql($"INSERT INTO `skills` (`Name`) VALUES ('Communication');");
            migrationBuilder.Sql($"INSERT INTO `skills` (`Name`) VALUES ('Public Relations');");
            migrationBuilder.Sql($"INSERT INTO `skills` (`Name`) VALUES ('Recruitment');");
            migrationBuilder.Sql($"INSERT INTO `skills` (`Name`) VALUES ('Business Development');");
            migrationBuilder.Sql($"INSERT INTO `skills` (`Name`) VALUES ('Mergers & Acquisitions');");
            migrationBuilder.Sql($"INSERT INTO `skills` (`Name`) VALUES ('Talent Development');");
            migrationBuilder.Sql($"INSERT INTO `skills` (`Name`) VALUES ('Organisational Development');");
            migrationBuilder.Sql($"INSERT INTO `skills` (`Name`) VALUES ('Marketing');");
            migrationBuilder.Sql($"INSERT INTO `skills` (`Name`) VALUES ('Technology');");
            migrationBuilder.Sql($"INSERT INTO `skills` (`Name`) VALUES ('Succession Planning');");
            migrationBuilder.Sql($"INSERT INTO `skills` (`Name`) VALUES ('Scale');");
            migrationBuilder.Sql($"INSERT INTO `skills` (`Name`) VALUES ('Startups');");
            migrationBuilder.Sql($"INSERT INTO `skills` (`Name`) VALUES ('Director competencies');");

            migrationBuilder.Sql($"INSERT INTO `locations` (`Name`) VALUES ('Remote');");
            migrationBuilder.Sql($"INSERT INTO `locations` (`Name`) VALUES ('Scotland');");
            migrationBuilder.Sql($"INSERT INTO `locations` (`Name`) VALUES ('North East');");
            migrationBuilder.Sql($"INSERT INTO `locations` (`Name`) VALUES ('North West');");
            migrationBuilder.Sql($"INSERT INTO `locations` (`Name`) VALUES ('Yorkshire');");
            migrationBuilder.Sql($"INSERT INTO `locations` (`Name`) VALUES ('East Midlands');");
            migrationBuilder.Sql($"INSERT INTO `locations` (`Name`) VALUES ('West Midlands');");
            migrationBuilder.Sql($"INSERT INTO `locations` (`Name`) VALUES ('South East');");
            migrationBuilder.Sql($"INSERT INTO `locations` (`Name`) VALUES ('South West');");
            migrationBuilder.Sql($"INSERT INTO `locations` (`Name`) VALUES ('London');");
            migrationBuilder.Sql($"INSERT INTO `locations` (`Name`) VALUES ('Other');");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "JobArtifacts");

            migrationBuilder.DropTable(
                name: "JobIndustries");

            migrationBuilder.DropTable(
                name: "JobLocations");

            migrationBuilder.DropTable(
                name: "JobSkills");

            migrationBuilder.DropTable(
                name: "Artifacts");

            migrationBuilder.DropTable(
                name: "Industries");

            migrationBuilder.DropTable(
                name: "Locations");

            migrationBuilder.DropTable(
                name: "Jobs");

            migrationBuilder.DropTable(
                name: "Skills");
        }
    }
}
