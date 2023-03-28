using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CareerExplorer.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class changingJobSeekerVcanacyTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CvPath",
                table: "JobSeekerVacancies");

            migrationBuilder.AddColumn<byte[]>(
                name: "Cv",
                table: "JobSeekerVacancies",
                type: "varbinary(max)",
                nullable: false,
                defaultValue: new byte[0]);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Cv",
                table: "JobSeekerVacancies");

            migrationBuilder.AddColumn<string>(
                name: "CvPath",
                table: "JobSeekerVacancies",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
