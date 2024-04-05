using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace boostifysolution1.Migrations
{
    /// <inheritdoc />
    public partial class languages : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ProductTitle",
                table: "SupportItems",
                newName: "ProductName");

            migrationBuilder.RenameColumn(
                name: "ProductPrimaryImage",
                table: "SupportItems",
                newName: "ProductImageURL");

            migrationBuilder.AddColumn<int>(
                name: "Language",
                table: "ProductReviews",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Language",
                table: "ProductReviews");

            migrationBuilder.RenameColumn(
                name: "ProductName",
                table: "SupportItems",
                newName: "ProductTitle");

            migrationBuilder.RenameColumn(
                name: "ProductImageURL",
                table: "SupportItems",
                newName: "ProductPrimaryImage");
        }
    }
}
