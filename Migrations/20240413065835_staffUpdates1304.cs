using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace boostifysolution1.Migrations
{
    /// <inheritdoc />
    public partial class staffUpdates1304 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Country",
                table: "AdminStaffs");

            migrationBuilder.AddColumn<int>(
                name: "FirstTaskId",
                table: "AdminStaffs",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "ReferralCode",
                table: "AdminStaffs",
                type: "nvarchar(6)",
                maxLength: 6,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FirstTaskId",
                table: "AdminStaffs");

            migrationBuilder.DropColumn(
                name: "ReferralCode",
                table: "AdminStaffs");

            migrationBuilder.AddColumn<int>(
                name: "Country",
                table: "AdminStaffs",
                type: "int",
                nullable: true);
        }
    }
}
