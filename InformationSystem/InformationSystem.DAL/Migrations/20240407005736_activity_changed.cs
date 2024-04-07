using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InformationSystem.DAL.Migrations
{
    /// <inheritdoc />
    public partial class activity_changed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Activities_Evaluations_EvaluationId",
                table: "Activities");

            migrationBuilder.AddForeignKey(
                name: "FK_Activities_Evaluations_EvaluationId",
                table: "Activities",
                column: "EvaluationId",
                principalTable: "Evaluations",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Activities_Evaluations_EvaluationId",
                table: "Activities");

            migrationBuilder.AddForeignKey(
                name: "FK_Activities_Evaluations_EvaluationId",
                table: "Activities",
                column: "EvaluationId",
                principalTable: "Evaluations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
