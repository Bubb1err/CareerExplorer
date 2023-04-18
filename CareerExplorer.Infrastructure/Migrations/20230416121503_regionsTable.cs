using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CareerExplorer.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class regionsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cities_Countries_CountryId",
                table: "Cities");

            migrationBuilder.DropForeignKey(
                name: "FK_JobSeekers_Cities_CityId",
                table: "JobSeekers");

            migrationBuilder.DropForeignKey(
                name: "FK_JobSeekers_Countries_CountryId",
                table: "JobSeekers");

            migrationBuilder.DropIndex(
                name: "IX_JobSeekers_CityId",
                table: "JobSeekers");

            migrationBuilder.DropIndex(
                name: "IX_JobSeekers_CountryId",
                table: "JobSeekers");

            migrationBuilder.DropIndex(
                name: "IX_Cities_CountryId",
                table: "Cities");

            migrationBuilder.DropColumn(
                name: "CityId",
                table: "JobSeekers");

            migrationBuilder.DropColumn(
                name: "CountryId",
                table: "JobSeekers");

            migrationBuilder.RenameColumn(
                name: "CountryId",
                table: "Cities",
                newName: "RegionId");

            migrationBuilder.AddColumn<string>(
                name: "Code",
                table: "Countries",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Language",
                table: "Countries",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "Regions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CountryId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Regions", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Regions");

            migrationBuilder.DropColumn(
                name: "Code",
                table: "Countries");

            migrationBuilder.DropColumn(
                name: "Language",
                table: "Countries");

            migrationBuilder.RenameColumn(
                name: "RegionId",
                table: "Cities",
                newName: "CountryId");

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
                name: "FK_Cities_Countries_CountryId",
                table: "Cities",
                column: "CountryId",
                principalTable: "Countries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

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
    }
}
