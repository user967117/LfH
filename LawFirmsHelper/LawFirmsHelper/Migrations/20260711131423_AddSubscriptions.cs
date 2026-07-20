using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace LawFirmsHelper.Migrations
{
    /// <inheritdoc />
    public partial class AddSubscriptions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "SubscriptionEndsAt",
                table: "Firm",
                type: "timestamp with time zone",
                nullable: true);
            
            migrationBuilder.AddColumn<int>(
                name: "SubscriptionPlanId",
                table: "Firm",
                type: "integer",
                nullable: true);
            
            migrationBuilder.CreateTable(
                name: "SubscriptionPlan",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Price = table.Column<decimal>(type: "numeric", nullable: false),
                    MaxLeeds = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubscriptionPlan", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Firm_SubscriptionPlanId",
                table: "Firm",
                column: "SubscriptionPlanId");

            migrationBuilder.AddForeignKey(
                name: "FK_Firm_SubscriptionPlan_SubscriptionPlanId",
                table: "Firm",
                column: "SubscriptionPlanId",
                principalTable: "SubscriptionPlan",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Firm_SubscriptionPlan_SubscriptionPlanId",
                table: "Firm");

            migrationBuilder.DropTable(
                name: "SubscriptionPlan");

            migrationBuilder.DropIndex(
                name: "IX_Firm_SubscriptionPlanId",
                table: "Firm");

            migrationBuilder.DropColumn(
                name: "SubscriptionEndsAt",
                table: "Firm");

            migrationBuilder.DropColumn(
                name: "SubscriptionPlanId",
                table: "Firm");
        }
    }
}