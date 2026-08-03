using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LawFirmsHelper.Migrations
{
    /// <inheritdoc />
    public partial class AddActors : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ActorId",
                table: "Messages",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "ActorId",
                table: "Leads",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "Actor",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Type = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Actor", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Messages_ActorId",
                table: "Messages",
                column: "ActorId");

            migrationBuilder.CreateIndex(
                name: "IX_Leads_ActorId",
                table: "Leads",
                column: "ActorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Leads_Actor_ActorId",
                table: "Leads",
                column: "ActorId",
                principalTable: "Actor",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_Actor_ActorId",
                table: "Messages",
                column: "ActorId",
                principalTable: "Actor",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Leads_Actor_ActorId",
                table: "Leads");

            migrationBuilder.DropForeignKey(
                name: "FK_Messages_Actor_ActorId",
                table: "Messages");

            migrationBuilder.DropTable(
                name: "Actor");

            migrationBuilder.DropIndex(
                name: "IX_Messages_ActorId",
                table: "Messages");

            migrationBuilder.DropIndex(
                name: "IX_Leads_ActorId",
                table: "Leads");

            migrationBuilder.DropColumn(
                name: "ActorId",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "ActorId",
                table: "Leads");
        }
    }
}
