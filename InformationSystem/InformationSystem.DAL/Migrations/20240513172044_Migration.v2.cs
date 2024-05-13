using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InformationSystem.DAL.Migrations
{
    /// <inheritdoc />
    public partial class Migrationv2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Activities_Evaluations_EvaluationId",
                table: "Activities");

            migrationBuilder.DropForeignKey(
                name: "FK_Evaluations_Students_StudentId",
                table: "Evaluations");

            migrationBuilder.DropIndex(
                name: "IX_Activities_EvaluationId",
                table: "Activities");

            migrationBuilder.DropColumn(
                name: "EvaluationId",
                table: "Activities");

            migrationBuilder.AlterColumn<Guid>(
                name: "StudentId",
                table: "Evaluations",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Evaluations",
                type: "TEXT",
                maxLength: 1000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 1000);

            migrationBuilder.AlterColumn<Guid>(
                name: "ActivityId",
                table: "Evaluations",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "TEXT");

            migrationBuilder.AddColumn<int>(
                name: "MaxPoints",
                table: "Activities",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Evaluations_ActivityId",
                table: "Evaluations",
                column: "ActivityId");

            migrationBuilder.AddForeignKey(
                name: "FK_Evaluations_Activities_ActivityId",
                table: "Evaluations",
                column: "ActivityId",
                principalTable: "Activities",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Evaluations_Students_StudentId",
                table: "Evaluations",
                column: "StudentId",
                principalTable: "Students",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Evaluations_Activities_ActivityId",
                table: "Evaluations");

            migrationBuilder.DropForeignKey(
                name: "FK_Evaluations_Students_StudentId",
                table: "Evaluations");

            migrationBuilder.DropIndex(
                name: "IX_Evaluations_ActivityId",
                table: "Evaluations");

            migrationBuilder.DropColumn(
                name: "MaxPoints",
                table: "Activities");

            migrationBuilder.AlterColumn<Guid>(
                name: "StudentId",
                table: "Evaluations",
                type: "TEXT",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Evaluations",
                type: "TEXT",
                maxLength: 1000,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 1000,
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "ActivityId",
                table: "Evaluations",
                type: "TEXT",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "EvaluationId",
                table: "Activities",
                type: "TEXT",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Activities_EvaluationId",
                table: "Activities",
                column: "EvaluationId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Activities_Evaluations_EvaluationId",
                table: "Activities",
                column: "EvaluationId",
                principalTable: "Evaluations",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Evaluations_Students_StudentId",
                table: "Evaluations",
                column: "StudentId",
                principalTable: "Students",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
