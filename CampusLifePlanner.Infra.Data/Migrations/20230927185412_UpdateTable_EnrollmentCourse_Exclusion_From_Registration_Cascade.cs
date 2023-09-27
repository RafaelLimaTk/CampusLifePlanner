using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CampusLifePlanner.Infra.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTable_EnrollmentCourse_Exclusion_From_Registration_Cascade : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_EnrollmentCourse_CourseId",
                table: "EnrollmentCourse",
                column: "CourseId");

            migrationBuilder.AddForeignKey(
                name: "FK_EnrollmentCourse_Courses_CourseId",
                table: "EnrollmentCourse",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EnrollmentCourse_Courses_CourseId",
                table: "EnrollmentCourse");

            migrationBuilder.DropIndex(
                name: "IX_EnrollmentCourse_CourseId",
                table: "EnrollmentCourse");
        }
    }
}
