using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CareerExplorer.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class changingVacancy : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Vacancies_Positions_PositionId",
                table: "Vacancies");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkTypes_Vacancies_VacancyId",
                table: "WorkTypes");

            migrationBuilder.DropIndex(
                name: "IX_WorkTypes_VacancyId",
                table: "WorkTypes");

            migrationBuilder.DropIndex(
                name: "IX_Vacancies_PositionId",
                table: "Vacancies");

            migrationBuilder.DropColumn(
                name: "PositionId",
                table: "Vacancies");

            migrationBuilder.CreateIndex(
                name: "IX_WorkTypes_VacancyId",
                table: "WorkTypes",
                column: "VacancyId");

            migrationBuilder.AddForeignKey(
                name: "FK_WorkTypes_Vacancies_VacancyId",
                table: "WorkTypes",
                column: "VacancyId",
                principalTable: "Vacancies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkTypes_Vacancies_VacancyId",
                table: "WorkTypes");

            migrationBuilder.DropIndex(
                name: "IX_WorkTypes_VacancyId",
                table: "WorkTypes");

            migrationBuilder.AddColumn<int>(
                name: "PositionId",
                table: "Vacancies",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_WorkTypes_VacancyId",
                table: "WorkTypes",
                column: "VacancyId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Vacancies_PositionId",
                table: "Vacancies",
                column: "PositionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Vacancies_Positions_PositionId",
                table: "Vacancies",
                column: "PositionId",
                principalTable: "Positions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkTypes_Vacancies_VacancyId",
                table: "WorkTypes",
                column: "VacancyId",
                principalTable: "Vacancies",
                principalColumn: "Id");
        }
    }
}
