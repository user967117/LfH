using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace LawFirmsHelper.Migrations
{
    /// <inheritdoc />
    public partial class CleanArchitecture : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Subscriptions_FirmId",
                table: "Subscriptions");

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Subscriptions",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CurrencyId",
                table: "Plans",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Currencies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Code = table.Column<string>(type: "text", nullable: false),
                    Symbol = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Currencies", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Subscriptions_FirmId",
                table: "Subscriptions",
                column: "FirmId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Plans_CurrencyId",
                table: "Plans",
                column: "CurrencyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Plans_Currencies_CurrencyId",
                table: "Plans",
                column: "CurrencyId",
                principalTable: "Currencies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Plans_Currencies_CurrencyId",
                table: "Plans");

            migrationBuilder.DropTable(
                name: "Currencies");

            migrationBuilder.DropIndex(
                name: "IX_Subscriptions_FirmId",
                table: "Subscriptions");

            migrationBuilder.DropIndex(
                name: "IX_Plans_CurrencyId",
                table: "Plans");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Subscriptions");

            migrationBuilder.DropColumn(
                name: "CurrencyId",
                table: "Plans");

            migrationBuilder.CreateIndex(
                name: "IX_Subscriptions_FirmId",
                table: "Subscriptions",
                column: "FirmId");
        }
    }
}
