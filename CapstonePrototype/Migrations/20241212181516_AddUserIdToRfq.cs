using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CapstonePrototype.Migrations
{
    /// <inheritdoc />
    public partial class AddUserIdToRfq : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Rfqs",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Rfqs_UserId",
                table: "Rfqs",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Rfqs_Users_UserId",
                table: "Rfqs",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rfqs_Users_UserId",
                table: "Rfqs");

            migrationBuilder.DropIndex(
                name: "IX_Rfqs_UserId",
                table: "Rfqs");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Rfqs");
        }
    }
}
