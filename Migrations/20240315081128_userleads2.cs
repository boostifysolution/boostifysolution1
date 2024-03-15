using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace boostifysolution1.Migrations
{
    /// <inheritdoc />
    public partial class userleads2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserLeads_AdminStaffs_AdminStaffId",
                table: "UserLeads");

            migrationBuilder.DropIndex(
                name: "IX_UserLeads_AdminStaffId",
                table: "UserLeads");

            migrationBuilder.AlterColumn<int>(
                name: "AdminStaffId",
                table: "UserLeads",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "AdminStaffId",
                table: "UserLeads",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserLeads_AdminStaffId",
                table: "UserLeads",
                column: "AdminStaffId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserLeads_AdminStaffs_AdminStaffId",
                table: "UserLeads",
                column: "AdminStaffId",
                principalTable: "AdminStaffs",
                principalColumn: "AdminStaffId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
