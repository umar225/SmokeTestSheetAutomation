using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Coursewise.Data.Migrations
{
    public partial class Industry_Skill_Location_NoOfJobs : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "NoOfJob",
                table: "Skills",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "NoOfJob",
                table: "Locations",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "NoOfJob",
                table: "Industries",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.Sql($"update skills,(select s.Id,(select count(js.SkillId) from jobskills as js where js.SkillId=s.Id) as NoOfJob from skills as s) as tmptable set skills.NoOfJob=tmptable.NoOfJob where skills.Id=tmptable.Id;");
            migrationBuilder.Sql($"update locations,(select l.id,(select count(jl.LocationId) from joblocations as jl where jl.LocationId=l.Id) as NoOfJob from locations as l) as tmptable set locations.NoOfJob=tmptable.NoOfJob where locations.Id=tmptable.Id;");
            migrationBuilder.Sql($"update industries,(select i.id,(select count(ji.IndustryId) from jobindustries as ji where ji.IndustryId=i.Id) as NoOfJob from industries as i) as tmptable set industries.NoOfJob=tmptable.NoOfJob where industries.Id=tmptable.Id;");

            migrationBuilder.Sql($"CREATE TRIGGER `Jobskills_AFTER_INSERT` AFTER INSERT ON `Jobskills` FOR EACH ROW BEGIN update skills set NoOfJob=NoOfJob+1 where id = new.skillId; END");
            migrationBuilder.Sql($"CREATE TRIGGER `Jobskills_AFTER_DELETE` AFTER DELETE ON `Jobskills` FOR EACH ROW BEGIN update skills set NoOfJob=NoOfJob-1 where id = old.skillId; END");

            migrationBuilder.Sql($"CREATE TRIGGER `JobLocations_AFTER_INSERT` AFTER INSERT ON `JobLocations` FOR EACH ROW BEGIN update Locations set NoOfJob=NoOfJob+1 where id = new.LocationId; END");
            migrationBuilder.Sql($"CREATE TRIGGER `JobLocations_AFTER_DELETE` AFTER DELETE ON `JobLocations` FOR EACH ROW BEGIN update Locations set NoOfJob=NoOfJob-1 where id = old.LocationId; END");

            migrationBuilder.Sql($"CREATE TRIGGER `JobIndustries_AFTER_INSERT` AFTER INSERT ON `JobIndustries` FOR EACH ROW BEGIN update Industries set NoOfJob=NoOfJob+1 where id = new.IndustryId; END");
            migrationBuilder.Sql($"CREATE TRIGGER `JobIndustries_AFTER_DELETE` AFTER DELETE ON `JobIndustries` FOR EACH ROW BEGIN update Industries set NoOfJob=NoOfJob-1 where id = old.IndustryId; END");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql($"DROP TRIGGER `Jobskills_AFTER_INSERT`");
            migrationBuilder.Sql($"DROP TRIGGER `Jobskills_AFTER_DELETE`");

            migrationBuilder.Sql($"DROP TRIGGER `JobLocations_AFTER_INSERT`");
            migrationBuilder.Sql($"DROP TRIGGER `JobLocations_AFTER_DELETE`");

            migrationBuilder.Sql($"DROP TRIGGER `JobIndustries_AFTER_INSERT`");
            migrationBuilder.Sql($"DROP TRIGGER `JobIndustries_AFTER_DELETE`");

            migrationBuilder.DropColumn(
                name: "NoOfJob",
                table: "Skills");

            migrationBuilder.DropColumn(
                name: "NoOfJob",
                table: "Locations");

            migrationBuilder.DropColumn(
                name: "NoOfJob",
                table: "Industries");
        }
    }
}
