using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Coursewise.Data.Migrations
{
    public partial class Resource_ResourceArtifact : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Resources",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Title = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Preview = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Description = table.Column<string>(type: "longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Length = table.Column<int>(type: "int", nullable: false),
                    IsFeatured = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    IsReady = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    NoOfClaps = table.Column<int>(type: "int", nullable: false),
                    Url = table.Column<string>(type: "varchar(300)", maxLength: 300, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Resources", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ResourceArtifacts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ResourceId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    ArtifactId = table.Column<int>(type: "int", nullable: false),
                    Type = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ParentResourceArtifactId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResourceArtifacts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ResourceArtifacts_Artifacts_ArtifactId",
                        column: x => x.ArtifactId,
                        principalTable: "Artifacts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ResourceArtifacts_ResourceArtifacts_ParentResourceArtifactId",
                        column: x => x.ParentResourceArtifactId,
                        principalTable: "ResourceArtifacts",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ResourceArtifacts_Resources_ResourceId",
                        column: x => x.ResourceId,
                        principalTable: "Resources",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_ResourceArtifacts_ArtifactId",
                table: "ResourceArtifacts",
                column: "ArtifactId");

            migrationBuilder.CreateIndex(
                name: "IX_ResourceArtifacts_ParentResourceArtifactId",
                table: "ResourceArtifacts",
                column: "ParentResourceArtifactId");

            migrationBuilder.CreateIndex(
                name: "IX_ResourceArtifacts_ResourceId",
                table: "ResourceArtifacts",
                column: "ResourceId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ResourceArtifacts");

            migrationBuilder.DropTable(
                name: "Resources");
        }
    }
}
