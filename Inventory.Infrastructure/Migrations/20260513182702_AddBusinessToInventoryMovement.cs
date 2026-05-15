using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Inventory.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddBusinessToInventoryMovement : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "BusinessId",
                table: "InventoryMovements",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_InventoryMovements_BusinessId",
                table: "InventoryMovements",
                column: "BusinessId");

            migrationBuilder.AddForeignKey(
                name: "FK_InventoryMovements_Businesss_BusinessId",
                table: "InventoryMovements",
                column: "BusinessId",
                principalTable: "Businesss",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InventoryMovements_Businesss_BusinessId",
                table: "InventoryMovements");

            migrationBuilder.DropIndex(
                name: "IX_InventoryMovements_BusinessId",
                table: "InventoryMovements");

            migrationBuilder.DropColumn(
                name: "BusinessId",
                table: "InventoryMovements");
        }
    }
}
