using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CareerExplorer.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class cityCountryRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CityId",
                table: "Vacancies",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CountryId",
                table: "Vacancies",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CityId",
                table: "JobSeekers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CountryId",
                table: "JobSeekers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Vacancies_CityId",
                table: "Vacancies",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_Vacancies_CountryId",
                table: "Vacancies",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_JobSeekers_CityId",
                table: "JobSeekers",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_JobSeekers_CountryId",
                table: "JobSeekers",
                column: "CountryId");

            migrationBuilder.AddForeignKey(
                name: "FK_JobSeekers_Cities_CityId",
                table: "JobSeekers",
                column: "CityId",
                principalTable: "Cities",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_JobSeekers_Countries_CountryId",
                table: "JobSeekers",
                column: "CountryId",
                principalTable: "Countries",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Vacancies_Cities_CityId",
                table: "Vacancies",
                column: "CityId",
                principalTable: "Cities",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Vacancies_Countries_CountryId",
                table: "Vacancies",
                column: "CountryId",
                principalTable: "Countries",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_JobSeekers_Cities_CityId",
                table: "JobSeekers");

            migrationBuilder.DropForeignKey(
                name: "FK_JobSeekers_Countries_CountryId",
                table: "JobSeekers");

            migrationBuilder.DropForeignKey(
                name: "FK_Vacancies_Cities_CityId",
                table: "Vacancies");

            migrationBuilder.DropForeignKey(
                name: "FK_Vacancies_Countries_CountryId",
                table: "Vacancies");

            migrationBuilder.DropIndex(
                name: "IX_Vacancies_CityId",
                table: "Vacancies");

            migrationBuilder.DropIndex(
                name: "IX_Vacancies_CountryId",
                table: "Vacancies");

            migrationBuilder.DropIndex(
                name: "IX_JobSeekers_CityId",
                table: "JobSeekers");

            migrationBuilder.DropIndex(
                name: "IX_JobSeekers_CountryId",
                table: "JobSeekers");

            migrationBuilder.DropColumn(
                name: "CityId",
                table: "Vacancies");

            migrationBuilder.DropColumn(
                name: "CountryId",
                table: "Vacancies");

            migrationBuilder.DropColumn(
                name: "CityId",
                table: "JobSeekers");

            migrationBuilder.DropColumn(
                name: "CountryId",
                table: "JobSeekers");
        }
    }
}
