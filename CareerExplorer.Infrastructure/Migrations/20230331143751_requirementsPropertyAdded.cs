using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CareerExplorer.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class requirementsPropertyAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SkillsTags_Vacancies_VacancyId",
                table: "SkillsTags");

            migrationBuilder.DropIndex(
                name: "IX_SkillsTags_VacancyId",
                table: "SkillsTags");

            migrationBuilder.DropColumn(
                name: "VacancyId",
                table: "SkillsTags");

            migrationBuilder.CreateTable(
                name: "SkillsTagVacancy",
                columns: table => new
                {
                    RequirementsId = table.Column<int>(type: "int", nullable: false),
                    VacanciesId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SkillsTagVacancy", x => new { x.RequirementsId, x.VacanciesId });
                    table.ForeignKey(
                        name: "FK_SkillsTagVacancy_SkillsTags_RequirementsId",
                        column: x => x.RequirementsId,
                        principalTable: "SkillsTags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SkillsTagVacancy_Vacancies_VacanciesId",
                        column: x => x.VacanciesId,
                        principalTable: "Vacancies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SkillsTagVacancy_VacanciesId",
                table: "SkillsTagVacancy",
                column: "VacanciesId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SkillsTagVacancy");

            migrationBuilder.AddColumn<int>(
                name: "VacancyId",
                table: "SkillsTags",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SkillsTags_VacancyId",
                table: "SkillsTags",
                column: "VacancyId");

            migrationBuilder.AddForeignKey(
                name: "FK_SkillsTags_Vacancies_VacancyId",
                table: "SkillsTags",
                column: "VacancyId",
                principalTable: "Vacancies",
                principalColumn: "Id");
        }
    }
}
