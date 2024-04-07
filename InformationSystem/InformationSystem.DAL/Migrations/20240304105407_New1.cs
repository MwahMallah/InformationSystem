using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InformationSystem.DAL.Migrations
{
    /// <inheritdoc />
    public partial class New1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CourseEntityStudentEntity_Courses_CoursesId",
                table: "CourseEntityStudentEntity");

            migrationBuilder.DropForeignKey(
                name: "FK_CourseEntityStudentEntity_Students_StudentsId",
                table: "CourseEntityStudentEntity");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CourseEntityStudentEntity",
                table: "CourseEntityStudentEntity");

            migrationBuilder.RenameTable(
                name: "CourseEntityStudentEntity",
                newName: "StudentCoouse");

            migrationBuilder.RenameIndex(
                name: "IX_CourseEntityStudentEntity_StudentsId",
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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
                newName: "CourseEntityStudentEntity");

            migrationBuilder.RenameIndex(
                name: "IX_StudentCoouse_StudentsId",
                table: "CourseEntityStudentEntity",
                newName: "IX_CourseEntityStudentEntity_StudentsId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CourseEntityStudentEntity",
                table: "CourseEntityStudentEntity",
                columns: new[] { "CoursesId", "StudentsId" });

            migrationBuilder.AddForeignKey(
                name: "FK_CourseEntityStudentEntity_Courses_CoursesId",
                table: "CourseEntityStudentEntity",
                column: "CoursesId",
                principalTable: "Courses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CourseEntityStudentEntity_Students_StudentsId",
                table: "CourseEntityStudentEntity",
                column: "StudentsId",
                principalTable: "Students",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
