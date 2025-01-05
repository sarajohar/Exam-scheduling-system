using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExamSchedulingSystem.Migrations
{
    /// <inheritdoc />
    public partial class AddNewTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ClassRooms",
                columns: table => new
                {
                    room_id = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    capacity = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClassRooms", x => x.room_id);
                });

            migrationBuilder.CreateTable(
                name: "ExamSchedules",
                columns: table => new
                {
                    schedule_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    course_id = table.Column<int>(type: "int", nullable: false),
                    course_name = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false),
                    exam_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    start_time = table.Column<TimeSpan>(type: "time", nullable: false),
                    end_time = table.Column<TimeSpan>(type: "time", nullable: false),
                    place = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExamSchedules", x => x.schedule_id);
                    table.ForeignKey(
                        name: "FK_ExamSchedules_Courses_course_id",
                        column: x => x.course_id,
                        principalTable: "Courses",
                        principalColumn: "course_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TimeSlots",
                columns: table => new
                {
                    slot_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    day = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    start_time = table.Column<TimeSpan>(type: "time", nullable: false),
                    end_time = table.Column<TimeSpan>(type: "time", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TimeSlots", x => x.slot_id);
                });

            migrationBuilder.CreateTable(
                name: "ExamReservations",
                columns: table => new
                {
                    reservation_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    course_id = table.Column<int>(type: "int", nullable: false),
                    course_name = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false),
                    exam_type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    exam_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    start_time = table.Column<TimeSpan>(type: "time", nullable: false),
                    end_time = table.Column<TimeSpan>(type: "time", nullable: false),
                    room_id = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    coordinator_id = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExamReservations", x => x.reservation_id);
                    table.ForeignKey(
                        name: "FK_ExamReservations_ClassRooms_room_id",
                        column: x => x.room_id,
                        principalTable: "ClassRooms",
                        principalColumn: "room_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ExamReservations_Courses_course_id",
                        column: x => x.course_id,
                        principalTable: "Courses",
                        principalColumn: "course_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ExamReservations_Users_coordinator_id",
                        column: x => x.coordinator_id,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Lectures",
                columns: table => new
                {
                    lecture_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    course_id = table.Column<int>(type: "int", nullable: false),
                    slot_id = table.Column<int>(type: "int", nullable: false),
                    room_id = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lectures", x => x.lecture_id);
                    table.ForeignKey(
                        name: "FK_Lectures_ClassRooms_room_id",
                        column: x => x.room_id,
                        principalTable: "ClassRooms",
                        principalColumn: "room_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Lectures_Courses_course_id",
                        column: x => x.course_id,
                        principalTable: "Courses",
                        principalColumn: "course_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Lectures_TimeSlots_slot_id",
                        column: x => x.slot_id,
                        principalTable: "TimeSlots",
                        principalColumn: "slot_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ExamReservations_coordinator_id",
                table: "ExamReservations",
                column: "coordinator_id");

            migrationBuilder.CreateIndex(
                name: "IX_ExamReservations_course_id",
                table: "ExamReservations",
                column: "course_id");

            migrationBuilder.CreateIndex(
                name: "IX_ExamReservations_room_id",
                table: "ExamReservations",
                column: "room_id");

            migrationBuilder.CreateIndex(
                name: "IX_ExamSchedules_course_id",
                table: "ExamSchedules",
                column: "course_id");

            migrationBuilder.CreateIndex(
                name: "IX_Lectures_course_id",
                table: "Lectures",
                column: "course_id");

            migrationBuilder.CreateIndex(
                name: "IX_Lectures_room_id",
                table: "Lectures",
                column: "room_id");

            migrationBuilder.CreateIndex(
                name: "IX_Lectures_slot_id",
                table: "Lectures",
                column: "slot_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ExamReservations");

            migrationBuilder.DropTable(
                name: "ExamSchedules");

            migrationBuilder.DropTable(
                name: "Lectures");

            migrationBuilder.DropTable(
                name: "ClassRooms");

            migrationBuilder.DropTable(
                name: "TimeSlots");
        }
    }
}
