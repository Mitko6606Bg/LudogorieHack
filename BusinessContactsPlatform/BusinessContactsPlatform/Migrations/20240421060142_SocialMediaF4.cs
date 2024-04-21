using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BusinessContactsPlatform.Migrations
{
    /// <inheritdoc />
    public partial class SocialMediaF4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FacebookLink",
                table: "SocialMedia",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FacebookNames",
                table: "SocialMedia",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FacebookUsername",
                table: "SocialMedia",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "InstagramLink",
                table: "SocialMedia",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "InstagramNames",
                table: "SocialMedia",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "InstagramUsername",
                table: "SocialMedia",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "LinkedInLink",
                table: "SocialMedia",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "LinkedInNames",
                table: "SocialMedia",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "LinkedInUsername",
                table: "SocialMedia",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FacebookLink",
                table: "SocialMedia");

            migrationBuilder.DropColumn(
                name: "FacebookNames",
                table: "SocialMedia");

            migrationBuilder.DropColumn(
                name: "FacebookUsername",
                table: "SocialMedia");

            migrationBuilder.DropColumn(
                name: "InstagramLink",
                table: "SocialMedia");

            migrationBuilder.DropColumn(
                name: "InstagramNames",
                table: "SocialMedia");

            migrationBuilder.DropColumn(
                name: "InstagramUsername",
                table: "SocialMedia");

            migrationBuilder.DropColumn(
                name: "LinkedInLink",
                table: "SocialMedia");

            migrationBuilder.DropColumn(
                name: "LinkedInNames",
                table: "SocialMedia");

            migrationBuilder.DropColumn(
                name: "LinkedInUsername",
                table: "SocialMedia");
        }
    }
}
