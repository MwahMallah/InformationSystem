﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InformationSystem.DAL.Migrations
{
    /// <inheritdoc />
    public partial class activityEntityChanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Activities_Courses_CourseId",
                table: "Activities");

            migrationBuilder.AlterColumn<Guid>(
                name: "CourseId",
                table: "Activities",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "TEXT");

            migrationBuilder.AddForeignKey(
                name: "FK_Activities_Courses_CourseId",
                table: "Activities",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Activities_Courses_CourseId",
                table: "Activities");

            migrationBuilder.AlterColumn<Guid>(
                name: "CourseId",
                table: "Activities",
                type: "TEXT",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Activities_Courses_CourseId",
                table: "Activities",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
