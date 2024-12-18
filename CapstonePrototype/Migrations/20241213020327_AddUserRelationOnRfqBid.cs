using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CapstonePrototype.Migrations
{
    /// <inheritdoc />
    public partial class AddUserRelationOnRfqBid : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "RfqBids",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_RfqBids_UserId",
                table: "RfqBids",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_RfqBids_Users_UserId",
                table: "RfqBids",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RfqBids_Users_UserId",
                table: "RfqBids");

            migrationBuilder.DropIndex(
                name: "IX_RfqBids_UserId",
                table: "RfqBids");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "RfqBids");
        }
    }
}
