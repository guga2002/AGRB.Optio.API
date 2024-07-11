using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AGRB.Optio.Domain.Migrations
{
    /// <inheritdoc />
    public partial class AddAI : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Fraudable_Or_Not",
                table: "Transactions",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "SuspecisiousStatus",
                table: "Transactions",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Fraudable_Or_Not",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "SuspecisiousStatus",
                table: "Transactions");
        }
    }
}
