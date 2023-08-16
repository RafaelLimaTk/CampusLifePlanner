using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CampusLifePlanner.Infra.Data.Migrations
{
    /// <inheritdoc />
    public partial class alterando_campo_enrollmeent_course_id : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_EnrollmentCourse",
                table: "EnrollmentCourse");

            migrationBuilder.AddPrimaryKey(
                name: "PK_EnrollmentCourse",
                table: "EnrollmentCourse",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_EnrollmentCourse",
                table: "EnrollmentCourse");

            migrationBuilder.AddPrimaryKey(
                name: "PK_EnrollmentCourse",
                table: "EnrollmentCourse",
                column: "CourseId");
        }
    }
}
