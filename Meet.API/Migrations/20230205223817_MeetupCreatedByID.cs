using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Meet.API.Migrations
{
    /// <inheritdoc />
    public partial class MeetupCreatedByID : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CreatedById",
                table: "Meetups",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Meetups_CreatedById",
                table: "Meetups",
                column: "CreatedById");

            migrationBuilder.AddForeignKey(
                name: "FK_Meetups_Users_CreatedById",
                table: "Meetups",
                column: "CreatedById",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Meetups_Users_CreatedById",
                table: "Meetups");

            migrationBuilder.DropIndex(
                name: "IX_Meetups_CreatedById",
                table: "Meetups");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "Meetups");
        }
    }
}
