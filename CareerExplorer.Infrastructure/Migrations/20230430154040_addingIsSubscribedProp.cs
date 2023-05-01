using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CareerExplorer.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addingIsSubscribedProp : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsSubscribedToNotification",
                table: "JobSeekers",
                type: "bit",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsSubscribedToNotification",
                table: "JobSeekers");
        }
    }
}
