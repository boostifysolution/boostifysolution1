using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace boostifysolution1.Migrations
{
    /// <inheritdoc />
    public partial class leaderStaff : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LeaderAdminStaffs",
                columns: table => new
                {
                    AdminStaffId = table.Column<int>(type: "int", nullable: false),
                    AssociatedAdminStaffId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LeaderAdminStaffs", x => new { x.AdminStaffId, x.AssociatedAdminStaffId });
                    table.ForeignKey(
                        name: "FK_LeaderAdminStaffs_AdminStaffs_AdminStaffId",
                        column: x => x.AdminStaffId,
                        principalTable: "AdminStaffs",
                        principalColumn: "AdminStaffId",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LeaderAdminStaffs");
        }
    }
}
