using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CareerExplorer.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class jobseekervacancy : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_JobSeekerVacancy_JobSeekers_JobSeekerId",
                table: "JobSeekerVacancy");

            migrationBuilder.DropForeignKey(
                name: "FK_JobSeekerVacancy_Vacancies_VacancyId",
                table: "JobSeekerVacancy");

            migrationBuilder.DropPrimaryKey(
                name: "PK_JobSeekerVacancy",
                table: "JobSeekerVacancy");

            migrationBuilder.RenameTable(
                name: "JobSeekerVacancy",
                newName: "JobSeekerVacancies");

            migrationBuilder.RenameIndex(
                name: "IX_JobSeekerVacancy_VacancyId",
                table: "JobSeekerVacancies",
                newName: "IX_JobSeekerVacancies_VacancyId");

            migrationBuilder.RenameIndex(
                name: "IX_JobSeekerVacancy_JobSeekerId",
                table: "JobSeekerVacancies",
                newName: "IX_JobSeekerVacancies_JobSeekerId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_JobSeekerVacancies",
                table: "JobSeekerVacancies",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_JobSeekerVacancies_JobSeekers_JobSeekerId",
                table: "JobSeekerVacancies",
                column: "JobSeekerId",
                principalTable: "JobSeekers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_JobSeekerVacancies_Vacancies_VacancyId",
                table: "JobSeekerVacancies",
                column: "VacancyId",
                principalTable: "Vacancies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_JobSeekerVacancies_JobSeekers_JobSeekerId",
                table: "JobSeekerVacancies");

            migrationBuilder.DropForeignKey(
                name: "FK_JobSeekerVacancies_Vacancies_VacancyId",
                table: "JobSeekerVacancies");

            migrationBuilder.DropPrimaryKey(
                name: "PK_JobSeekerVacancies",
                table: "JobSeekerVacancies");

            migrationBuilder.RenameTable(
                name: "JobSeekerVacancies",
                newName: "JobSeekerVacancy");

            migrationBuilder.RenameIndex(
                name: "IX_JobSeekerVacancies_VacancyId",
                table: "JobSeekerVacancy",
                newName: "IX_JobSeekerVacancy_VacancyId");

            migrationBuilder.RenameIndex(
                name: "IX_JobSeekerVacancies_JobSeekerId",
                table: "JobSeekerVacancy",
                newName: "IX_JobSeekerVacancy_JobSeekerId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_JobSeekerVacancy",
                table: "JobSeekerVacancy",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_JobSeekerVacancy_JobSeekers_JobSeekerId",
                table: "JobSeekerVacancy",
                column: "JobSeekerId",
                principalTable: "JobSeekers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_JobSeekerVacancy_Vacancies_VacancyId",
                table: "JobSeekerVacancy",
                column: "VacancyId",
                principalTable: "Vacancies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
