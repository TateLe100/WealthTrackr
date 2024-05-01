using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WealthTrackr.Data.Migrations
{
    /// <inheritdoc />
    public partial class TakingfieldoffTransactiontable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FkCategoryId",
                table: "Transactions");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FkCategoryId",
                table: "Transactions",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
