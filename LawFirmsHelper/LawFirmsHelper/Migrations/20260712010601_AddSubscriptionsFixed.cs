using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace LawFirmsHelper.Migrations
{
    /// <inheritdoc />
    public partial class AddSubscriptionsFixed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Firm_SubscriptionPlan_SubscriptionPlanId",
                table: "Firm");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SubscriptionPlan",
                table: "SubscriptionPlan");

            migrationBuilder.RenameTable(
                name: "SubscriptionPlan",
                newName: "SubscriptionPlans");

            migrationBuilder.RenameColumn(
                name: "MaxLeeds",
                table: "SubscriptionPlans",
                newName: "MaxLeads");

            migrationBuilder.AlterColumn<int>(
                name: "SubscriptionPlanId",
                table: "Firm",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SubscriptionPlans",
                table: "SubscriptionPlans",
                column: "Id");

            migrationBuilder.InsertData(
                table: "SubscriptionPlans",
                columns: new[] { "Id", "MaxLeads", "Name", "Price" },
                values: new object[,]
                {
                    { 1, 10, "Free", 0m },
                    { 2, 100, "Pro", 49.99m },
                    { 3, 9999, "Enterprise", 199.99m }
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Firm_SubscriptionPlans_SubscriptionPlanId",
                table: "Firm",
                column: "SubscriptionPlanId",
                principalTable: "SubscriptionPlans",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Firm_SubscriptionPlans_SubscriptionPlanId",
                table: "Firm");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SubscriptionPlans",
                table: "SubscriptionPlans");

            migrationBuilder.DeleteData(
                table: "SubscriptionPlans",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "SubscriptionPlans",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "SubscriptionPlans",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.RenameTable(
                name: "SubscriptionPlans",
                newName: "SubscriptionPlan");

            migrationBuilder.RenameColumn(
                name: "MaxLeads",
                table: "SubscriptionPlan",
                newName: "MaxLeeds");

            migrationBuilder.AlterColumn<int>(
                name: "SubscriptionPlanId",
                table: "Firm",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_SubscriptionPlan",
                table: "SubscriptionPlan",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Firm_SubscriptionPlan_SubscriptionPlanId",
                table: "Firm",
                column: "SubscriptionPlanId",
                principalTable: "SubscriptionPlan",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
