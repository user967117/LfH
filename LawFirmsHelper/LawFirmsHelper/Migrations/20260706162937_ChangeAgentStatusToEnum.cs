using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LawFirmsHelper.Migrations
{
    /// <inheritdoc />
    public partial class ChangeAgentStatusToEnum : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Agents");

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Agents",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "Agents");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Agents",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }
    }
}
