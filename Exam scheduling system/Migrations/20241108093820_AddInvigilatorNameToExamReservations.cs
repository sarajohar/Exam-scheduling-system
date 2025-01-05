using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExamSchedulingSystem.Migrations
{
    /// <inheritdoc />
    public partial class AddInvigilatorNameToExamReservations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "invigilator_name",
                table: "ExamReservations",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "invigilator_name",
                table: "ExamReservations");
        }
    }
}
