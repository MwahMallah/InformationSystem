using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InformationSystem.DAL.Migrations
{
    /// <inheritdoc />
    public partial class changed_student_entity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StudentCoouse_Courses_CoursesId",
                table: "StudentCoouse");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentCoouse_Students_StudentsId",
                table: "StudentCoouse");

            migrationBuilder.DropPrimaryKey(
                name: "PK_StudentCoouse",
                table: "StudentCoouse");

            migrationBuilder.RenameTable(
                name: "StudentCoouse",
                newName: "StudentCourse");

            migrationBuilder.RenameColumn(
                name: "CurrentYear",
                table: "Students",
                newName: "StartYear");

            migrationBuilder.RenameIndex(
                name: "IX_StudentCoouse_StudentsId",
                table: "StudentCourse",
                newName: "IX_StudentCourse_StudentsId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_StudentCourse",
                table: "StudentCourse",
                columns: new[] { "CoursesId", "StudentsId" });

            migrationBuilder.AddForeignKey(
                name: "FK_StudentCourse_Courses_CoursesId",
                table: "StudentCourse",
                column: "CoursesId",
                principalTable: "Courses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StudentCourse_Students_StudentsId",
                table: "StudentCourse",
                column: "StudentsId",
                principalTable: "Students",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StudentCourse_Courses_CoursesId",
                table: "StudentCourse");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentCourse_Students_StudentsId",
                table: "StudentCourse");

            migrationBuilder.DropPrimaryKey(
                name: "PK_StudentCourse",
                table: "StudentCourse");

            migrationBuilder.RenameTable(
                name: "StudentCourse",
                newName: "StudentCoouse");

            migrationBuilder.RenameColumn(
                name: "StartYear",
                table: "Students",
                newName: "CurrentYear");

            migrationBuilder.RenameIndex(
                name: "IX_StudentCourse_StudentsId",
                table: "StudentCoouse",
                newName: "IX_StudentCoouse_StudentsId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_StudentCoouse",
                table: "StudentCoouse",
                columns: new[] { "CoursesId", "StudentsId" });

            migrationBuilder.AddForeignKey(
                name: "FK_StudentCoouse_Courses_CoursesId",
                table: "StudentCoouse",
                column: "CoursesId",
                principalTable: "Courses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StudentCoouse_Students_StudentsId",
                table: "StudentCoouse",
                column: "StudentsId",
                principalTable: "Students",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
