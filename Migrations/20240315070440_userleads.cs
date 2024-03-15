using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace boostifysolution1.Migrations
{
    /// <inheritdoc />
    public partial class userleads : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserLeads",
                columns: table => new
                {
                    UserLeadId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AdminStaffId = table.Column<int>(type: "int", nullable: false),
                    LeadStatus = table.Column<int>(type: "int", nullable: false),
                    DateAdded = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Country = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserLeads", x => x.UserLeadId);
                    table.ForeignKey(
                        name: "FK_UserLeads_AdminStaffs_AdminStaffId",
                        column: x => x.AdminStaffId,
                        principalTable: "AdminStaffs",
                        principalColumn: "AdminStaffId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserLeads_AdminStaffId",
                table: "UserLeads",
                column: "AdminStaffId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserLeads");
        }
    }
}
