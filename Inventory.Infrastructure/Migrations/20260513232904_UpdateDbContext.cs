using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Inventory.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDbContext : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AuditHistories_Businesss_BusinessId",
                table: "AuditHistories");

            migrationBuilder.DropForeignKey(
                name: "FK_Branches_Businesss_BusinessId",
                table: "Branches");

            migrationBuilder.DropForeignKey(
                name: "FK_Categories_Businesss_BusinessId",
                table: "Categories");

            migrationBuilder.DropForeignKey(
                name: "FK_Customers_Businesss_BusinessId",
                table: "Customers");

            migrationBuilder.DropForeignKey(
                name: "FK_InventoryMovements_Businesss_BusinessId",
                table: "InventoryMovements");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_Businesss_BusinessId",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_Providers_Businesss_BusinessId",
                table: "Providers");

            migrationBuilder.DropForeignKey(
                name: "FK_Purchases_Businesss_BusinessId",
                table: "Purchases");

            migrationBuilder.DropForeignKey(
                name: "FK_Sales_Businesss_BusinessId",
                table: "Sales");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Businesss_BusinessId",
                table: "Users");

            migrationBuilder.DropForeignKey(
                name: "FK_Warehouses_Businesss_BusinessId",
                table: "Warehouses");

            migrationBuilder.AddForeignKey(
                name: "FK_AuditHistories_Businesss_BusinessId",
                table: "AuditHistories",
                column: "BusinessId",
                principalTable: "Businesss",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Branches_Businesss_BusinessId",
                table: "Branches",
                column: "BusinessId",
                principalTable: "Businesss",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Categories_Businesss_BusinessId",
                table: "Categories",
                column: "BusinessId",
                principalTable: "Businesss",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Customers_Businesss_BusinessId",
                table: "Customers",
                column: "BusinessId",
                principalTable: "Businesss",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_InventoryMovements_Businesss_BusinessId",
                table: "InventoryMovements",
                column: "BusinessId",
                principalTable: "Businesss",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Businesss_BusinessId",
                table: "Products",
                column: "BusinessId",
                principalTable: "Businesss",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Providers_Businesss_BusinessId",
                table: "Providers",
                column: "BusinessId",
                principalTable: "Businesss",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Purchases_Businesss_BusinessId",
                table: "Purchases",
                column: "BusinessId",
                principalTable: "Businesss",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Sales_Businesss_BusinessId",
                table: "Sales",
                column: "BusinessId",
                principalTable: "Businesss",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Businesss_BusinessId",
                table: "Users",
                column: "BusinessId",
                principalTable: "Businesss",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Warehouses_Businesss_BusinessId",
                table: "Warehouses",
                column: "BusinessId",
                principalTable: "Businesss",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AuditHistories_Businesss_BusinessId",
                table: "AuditHistories");

            migrationBuilder.DropForeignKey(
                name: "FK_Branches_Businesss_BusinessId",
                table: "Branches");

            migrationBuilder.DropForeignKey(
                name: "FK_Categories_Businesss_BusinessId",
                table: "Categories");

            migrationBuilder.DropForeignKey(
                name: "FK_Customers_Businesss_BusinessId",
                table: "Customers");

            migrationBuilder.DropForeignKey(
                name: "FK_InventoryMovements_Businesss_BusinessId",
                table: "InventoryMovements");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_Businesss_BusinessId",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_Providers_Businesss_BusinessId",
                table: "Providers");

            migrationBuilder.DropForeignKey(
                name: "FK_Purchases_Businesss_BusinessId",
                table: "Purchases");

            migrationBuilder.DropForeignKey(
                name: "FK_Sales_Businesss_BusinessId",
                table: "Sales");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Businesss_BusinessId",
                table: "Users");

            migrationBuilder.DropForeignKey(
                name: "FK_Warehouses_Businesss_BusinessId",
                table: "Warehouses");

            migrationBuilder.AddForeignKey(
                name: "FK_AuditHistories_Businesss_BusinessId",
                table: "AuditHistories",
                column: "BusinessId",
                principalTable: "Businesss",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Branches_Businesss_BusinessId",
                table: "Branches",
                column: "BusinessId",
                principalTable: "Businesss",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Categories_Businesss_BusinessId",
                table: "Categories",
                column: "BusinessId",
                principalTable: "Businesss",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Customers_Businesss_BusinessId",
                table: "Customers",
                column: "BusinessId",
                principalTable: "Businesss",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_InventoryMovements_Businesss_BusinessId",
                table: "InventoryMovements",
                column: "BusinessId",
                principalTable: "Businesss",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Businesss_BusinessId",
                table: "Products",
                column: "BusinessId",
                principalTable: "Businesss",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Providers_Businesss_BusinessId",
                table: "Providers",
                column: "BusinessId",
                principalTable: "Businesss",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Purchases_Businesss_BusinessId",
                table: "Purchases",
                column: "BusinessId",
                principalTable: "Businesss",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Sales_Businesss_BusinessId",
                table: "Sales",
                column: "BusinessId",
                principalTable: "Businesss",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Businesss_BusinessId",
                table: "Users",
                column: "BusinessId",
                principalTable: "Businesss",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Warehouses_Businesss_BusinessId",
                table: "Warehouses",
                column: "BusinessId",
                principalTable: "Businesss",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
