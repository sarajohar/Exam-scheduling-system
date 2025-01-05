using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExamSchedulingSystem.Migrations
{
    /// <inheritdoc />
    public partial class AddColumnsToLectures : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "number_of_students",
                table: "Lectures",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "user_id",
                table: "Lectures",
                type: "nvarchar(450)",
                maxLength: 450,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "user_role",
                table: "Lectures",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "number_of_students",
                table: "Lectures");

            migrationBuilder.DropColumn(
                name: "user_id",
                table: "Lectures");

            migrationBuilder.DropColumn(
                name: "user_role",
                table: "Lectures");
        }
    }
}
