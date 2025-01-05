using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExamSchedulingSystem.Migrations
{
    /// <inheritdoc />
    public partial class AddCoordinatorNameToExcuses : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CoordinatorName",
                table: "Excuses",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CoordinatorName",
                table: "Excuses");
        }
    }
}
