using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PeopleSkillsApp.Migrations
{
    /// <inheritdoc />
    public partial class AddLevelToPersonSkill : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Level",
                table: "Skills",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Level",
                table: "PersonSkills",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "PersonSkills",
                keyColumns: new[] { "PersonId", "SkillId" },
                keyValues: new object[] { 1, 1 },
                column: "Level",
                value: 0);

            migrationBuilder.UpdateData(
                table: "PersonSkills",
                keyColumns: new[] { "PersonId", "SkillId" },
                keyValues: new object[] { 1, 2 },
                column: "Level",
                value: 0);

            migrationBuilder.UpdateData(
                table: "PersonSkills",
                keyColumns: new[] { "PersonId", "SkillId" },
                keyValues: new object[] { 2, 1 },
                column: "Level",
                value: 0);

            migrationBuilder.UpdateData(
                table: "PersonSkills",
                keyColumns: new[] { "PersonId", "SkillId" },
                keyValues: new object[] { 2, 3 },
                column: "Level",
                value: 0);

            migrationBuilder.UpdateData(
                table: "PersonSkills",
                keyColumns: new[] { "PersonId", "SkillId" },
                keyValues: new object[] { 3, 3 },
                column: "Level",
                value: 0);

            migrationBuilder.UpdateData(
                table: "PersonSkills",
                keyColumns: new[] { "PersonId", "SkillId" },
                keyValues: new object[] { 3, 4 },
                column: "Level",
                value: 0);

            migrationBuilder.UpdateData(
                table: "PersonSkills",
                keyColumns: new[] { "PersonId", "SkillId" },
                keyValues: new object[] { 3, 5 },
                column: "Level",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 1,
                column: "Level",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 2,
                column: "Level",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 3,
                column: "Level",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 4,
                column: "Level",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 5,
                column: "Level",
                value: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Level",
                table: "Skills");

            migrationBuilder.DropColumn(
                name: "Level",
                table: "PersonSkills");
        }
    }
}
