using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CapstonePrototype.Migrations
{
    /// <inheritdoc />
    public partial class addIsCompletedOnRfq : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsCompleted",
                table: "Rfqs",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsCompleted",
                table: "Rfqs");
        }
    }
}
