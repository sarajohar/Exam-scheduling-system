using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExamSchedulingSystem.Migrations
{
    /// <inheritdoc />
    public partial class IncreasePlaceLength : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
migrationBuilder.AlterColumn<string>(
        name: "place",
        table: "ExamSchedules",
        type: "nvarchar(10)", // Revert back to the original size
        nullable: false,
        oldClrType: typeof(string),
        oldType: "nvarchar(255)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                    name: "place",
                    table: "ExamSchedules",
                    type: "nvarchar(10)", // Revert back to the original size
                    nullable: false,
                    oldClrType: typeof(string),
                    oldType: "nvarchar(255)");
        }
    }
}
