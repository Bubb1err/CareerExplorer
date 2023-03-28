using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CareerExplorer.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class conf : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_JobSeekerVacancies_JobSeekers_JobSeekerId",
                table: "JobSeekerVacancies");

            migrationBuilder.DropForeignKey(
                name: "FK_JobSeekerVacancies_Vacancies_VacancyId",
                table: "JobSeekerVacancies");

            migrationBuilder.DropForeignKey(
                name: "FK_Vacancies_Recruiters_CreatorId",
                table: "Vacancies");

            migrationBuilder.DropPrimaryKey(
                name: "PK_JobSeekerVacancies",
                table: "JobSeekerVacancies");

            migrationBuilder.DropIndex(
                name: "IX_JobSeekerVacancies_VacancyId",
                table: "JobSeekerVacancies");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "JobSeekerVacancies",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_JobSeekerVacancies",
                table: "JobSeekerVacancies",
                columns: new[] { "VacancyId", "JobSeekerId" });

            migrationBuilder.AddForeignKey(
                name: "FK_JobSeekerVacancies_JobSeekers_JobSeekerId",
                table: "JobSeekerVacancies",
                column: "JobSeekerId",
                principalTable: "JobSeekers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_JobSeekerVacancies_Vacancies_VacancyId",
                table: "JobSeekerVacancies",
                column: "VacancyId",
                principalTable: "Vacancies",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Vacancies_Recruiters_CreatorId",
                table: "Vacancies",
                column: "CreatorId",
                principalTable: "Recruiters",
                principalColumn: "Id");
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

            migrationBuilder.DropForeignKey(
                name: "FK_Vacancies_Recruiters_CreatorId",
                table: "Vacancies");

            migrationBuilder.DropPrimaryKey(
                name: "PK_JobSeekerVacancies",
                table: "JobSeekerVacancies");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "JobSeekerVacancies",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_JobSeekerVacancies",
                table: "JobSeekerVacancies",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_JobSeekerVacancies_VacancyId",
                table: "JobSeekerVacancies",
                column: "VacancyId");

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

            migrationBuilder.AddForeignKey(
                name: "FK_Vacancies_Recruiters_CreatorId",
                table: "Vacancies",
                column: "CreatorId",
                principalTable: "Recruiters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
