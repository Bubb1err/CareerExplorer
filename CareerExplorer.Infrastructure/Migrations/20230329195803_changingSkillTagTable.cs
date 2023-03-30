using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CareerExplorer.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class changingSkillTagTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SkillsTags_JobSeekers_JobSeekerId",
                table: "SkillsTags");

            migrationBuilder.DropIndex(
                name: "IX_SkillsTags_JobSeekerId",
                table: "SkillsTags");

            migrationBuilder.DropColumn(
                name: "JobSeekerId",
                table: "SkillsTags");

            migrationBuilder.CreateTable(
                name: "JobSeekerSkillsTag",
                columns: table => new
                {
                    JobSeekersId = table.Column<int>(type: "int", nullable: false),
                    SkillsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobSeekerSkillsTag", x => new { x.JobSeekersId, x.SkillsId });
                    table.ForeignKey(
                        name: "FK_JobSeekerSkillsTag_JobSeekers_JobSeekersId",
                        column: x => x.JobSeekersId,
                        principalTable: "JobSeekers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_JobSeekerSkillsTag_SkillsTags_SkillsId",
                        column: x => x.SkillsId,
                        principalTable: "SkillsTags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_JobSeekerSkillsTag_SkillsId",
                table: "JobSeekerSkillsTag",
                column: "SkillsId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "JobSeekerSkillsTag");

            migrationBuilder.AddColumn<int>(
                name: "JobSeekerId",
                table: "SkillsTags",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SkillsTags_JobSeekerId",
                table: "SkillsTags",
                column: "JobSeekerId");

            migrationBuilder.AddForeignKey(
                name: "FK_SkillsTags_JobSeekers_JobSeekerId",
                table: "SkillsTags",
                column: "JobSeekerId",
                principalTable: "JobSeekers",
                principalColumn: "Id");
        }
    }
}
