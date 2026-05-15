using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Inventory.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddBusinessIdToEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "BusinessId",
                table: "Warehouses",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "BusinessId",
                table: "Users",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "BusinessId",
                table: "Sales",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "BusinessId",
                table: "Purchases",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "BusinessId",
                table: "Providers",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "BusinessId",
                table: "Products",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "BusinessId",
                table: "Customers",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "BusinessId",
                table: "Categories",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "BusinessId",
                table: "Branches",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "BusinessId",
                table: "AuditHistories",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Warehouses_BusinessId",
                table: "Warehouses",
                column: "BusinessId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_BusinessId",
                table: "Users",
                column: "BusinessId");

            migrationBuilder.CreateIndex(
                name: "IX_Sales_BusinessId",
                table: "Sales",
                column: "BusinessId");

            migrationBuilder.CreateIndex(
                name: "IX_Purchases_BusinessId",
                table: "Purchases",
                column: "BusinessId");

            migrationBuilder.CreateIndex(
                name: "IX_Providers_BusinessId",
                table: "Providers",
                column: "BusinessId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_BusinessId",
                table: "Products",
                column: "BusinessId");

            migrationBuilder.CreateIndex(
                name: "IX_Customers_BusinessId",
                table: "Customers",
                column: "BusinessId");

            migrationBuilder.CreateIndex(
                name: "IX_Categories_BusinessId",
                table: "Categories",
                column: "BusinessId");

            migrationBuilder.CreateIndex(
                name: "IX_Branches_BusinessId",
                table: "Branches",
                column: "BusinessId");

            migrationBuilder.CreateIndex(
                name: "IX_AuditHistories_BusinessId",
                table: "AuditHistories",
                column: "BusinessId");

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

            migrationBuilder.DropIndex(
                name: "IX_Warehouses_BusinessId",
                table: "Warehouses");

            migrationBuilder.DropIndex(
                name: "IX_Users_BusinessId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Sales_BusinessId",
                table: "Sales");

            migrationBuilder.DropIndex(
                name: "IX_Purchases_BusinessId",
                table: "Purchases");

            migrationBuilder.DropIndex(
                name: "IX_Providers_BusinessId",
                table: "Providers");

            migrationBuilder.DropIndex(
                name: "IX_Products_BusinessId",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Customers_BusinessId",
                table: "Customers");

            migrationBuilder.DropIndex(
                name: "IX_Categories_BusinessId",
                table: "Categories");

            migrationBuilder.DropIndex(
                name: "IX_Branches_BusinessId",
                table: "Branches");

            migrationBuilder.DropIndex(
                name: "IX_AuditHistories_BusinessId",
                table: "AuditHistories");

            migrationBuilder.DropColumn(
                name: "BusinessId",
                table: "Warehouses");

            migrationBuilder.DropColumn(
                name: "BusinessId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "BusinessId",
                table: "Sales");

            migrationBuilder.DropColumn(
                name: "BusinessId",
                table: "Purchases");

            migrationBuilder.DropColumn(
                name: "BusinessId",
                table: "Providers");

            migrationBuilder.DropColumn(
                name: "BusinessId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "BusinessId",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "BusinessId",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "BusinessId",
                table: "Branches");

            migrationBuilder.DropColumn(
                name: "BusinessId",
                table: "AuditHistories");
        }
    }
}
