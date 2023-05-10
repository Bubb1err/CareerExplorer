using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CareerExplorer.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class changingJobSeeker : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Countries_Admins_AdminId",
                table: "Countries");

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

            migrationBuilder.AddColumn<int>(
                name: "EnglishLevel",
                table: "JobSeekers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ExperienceYears",
                table: "JobSeekers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LinkedIn",
                table: "JobSeekers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Salary",
                table: "JobSeekers",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "AdminId",
                table: "Countries",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateTable(
                name: "Cities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CountryId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Cities_Countries_CountryId",
                        column: x => x.CountryId,
                        principalTable: "Countries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_JobSeekers_CityId",
                table: "JobSeekers",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_JobSeekers_CountryId",
                table: "JobSeekers",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_Cities_CountryId",
                table: "Cities",
                column: "CountryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Countries_Admins_AdminId",
                table: "Countries",
                column: "AdminId",
                principalTable: "Admins",
                principalColumn: "Id");

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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Countries_Admins_AdminId",
                table: "Countries");

            migrationBuilder.DropForeignKey(
                name: "FK_JobSeekers_Cities_CityId",
                table: "JobSeekers");

            migrationBuilder.DropForeignKey(
                name: "FK_JobSeekers_Countries_CountryId",
                table: "JobSeekers");

            migrationBuilder.DropTable(
                name: "Cities");

            migrationBuilder.DropIndex(
                name: "IX_JobSeekers_CityId",
                table: "JobSeekers");

            migrationBuilder.DropIndex(
                name: "IX_JobSeekers_CountryId",
                table: "JobSeekers");

            migrationBuilder.DropColumn(
                name: "CityId",
                table: "JobSeekers");

            migrationBuilder.DropColumn(
                name: "CountryId",
                table: "JobSeekers");

            migrationBuilder.DropColumn(
                name: "EnglishLevel",
                table: "JobSeekers");

            migrationBuilder.DropColumn(
                name: "ExperienceYears",
                table: "JobSeekers");

            migrationBuilder.DropColumn(
                name: "LinkedIn",
                table: "JobSeekers");

            migrationBuilder.DropColumn(
                name: "Salary",
                table: "JobSeekers");

            migrationBuilder.AlterColumn<int>(
                name: "AdminId",
                table: "Countries",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Countries_Admins_AdminId",
                table: "Countries",
                column: "AdminId",
                principalTable: "Admins",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
