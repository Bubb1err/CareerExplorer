using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CareerExplorer.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class changingvacancy : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Recruiters_AspNetUsers_UserId",
                table: "Recruiters");

            migrationBuilder.DropForeignKey(
                name: "FK_Vacancies_Recruiters_RecruiterId",
                table: "Vacancies");

            migrationBuilder.DropIndex(
                name: "IX_Vacancies_RecruiterId",
                table: "Vacancies");

            migrationBuilder.DropIndex(
                name: "IX_Recruiters_UserId",
                table: "Recruiters");

            migrationBuilder.DropColumn(
                name: "RecruiterId",
                table: "Vacancies");

            migrationBuilder.RenameColumn(
                name: "Candidates",
                table: "Vacancies",
                newName: "CreatorId");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "Vacancies",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Recruiters",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<int>(
                name: "VacancyId",
                table: "JobSeekers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Vacancies_CreatorId",
                table: "Vacancies",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_JobSeekers_VacancyId",
                table: "JobSeekers",
                column: "VacancyId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_RecruiterProfileId",
                table: "AspNetUsers",
                column: "RecruiterProfileId",
                unique: true,
                filter: "[RecruiterProfileId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Recruiters_RecruiterProfileId",
                table: "AspNetUsers",
                column: "RecruiterProfileId",
                principalTable: "Recruiters",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_JobSeekers_Vacancies_VacancyId",
                table: "JobSeekers",
                column: "VacancyId",
                principalTable: "Vacancies",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Vacancies_Recruiters_CreatorId",
                table: "Vacancies",
                column: "CreatorId",
                principalTable: "Recruiters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Recruiters_RecruiterProfileId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_JobSeekers_Vacancies_VacancyId",
                table: "JobSeekers");

            migrationBuilder.DropForeignKey(
                name: "FK_Vacancies_Recruiters_CreatorId",
                table: "Vacancies");

            migrationBuilder.DropIndex(
                name: "IX_Vacancies_CreatorId",
                table: "Vacancies");

            migrationBuilder.DropIndex(
                name: "IX_JobSeekers_VacancyId",
                table: "JobSeekers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_RecruiterProfileId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "Vacancies");

            migrationBuilder.DropColumn(
                name: "VacancyId",
                table: "JobSeekers");

            migrationBuilder.RenameColumn(
                name: "CreatorId",
                table: "Vacancies",
                newName: "Candidates");

            migrationBuilder.AddColumn<int>(
                name: "RecruiterId",
                table: "Vacancies",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Recruiters",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_Vacancies_RecruiterId",
                table: "Vacancies",
                column: "RecruiterId");

            migrationBuilder.CreateIndex(
                name: "IX_Recruiters_UserId",
                table: "Recruiters",
                column: "UserId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Recruiters_AspNetUsers_UserId",
                table: "Recruiters",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Vacancies_Recruiters_RecruiterId",
                table: "Vacancies",
                column: "RecruiterId",
                principalTable: "Recruiters",
                principalColumn: "Id");
        }
    }
}
