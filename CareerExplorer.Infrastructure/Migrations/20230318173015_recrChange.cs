using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CareerExplorer.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class recrChange : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Recruiters_RecruiterProfileId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_Recruiters_Companies_CompanyId",
                table: "Recruiters");

            migrationBuilder.DropIndex(
                name: "IX_Recruiters_CompanyId",
                table: "Recruiters");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_RecruiterProfileId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "Recruiters");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Recruiters",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Recruiters_AspNetUsers_UserId",
                table: "Recruiters");

            migrationBuilder.DropIndex(
                name: "IX_Recruiters_UserId",
                table: "Recruiters");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Recruiters",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<int>(
                name: "CompanyId",
                table: "Recruiters",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Recruiters_CompanyId",
                table: "Recruiters",
                column: "CompanyId");

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
                name: "FK_Recruiters_Companies_CompanyId",
                table: "Recruiters",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
