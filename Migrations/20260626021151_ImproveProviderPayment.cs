using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DiaspoDirect.Migrations
{
    /// <inheritdoc />
    public partial class ImproveProviderPayment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProviderPayments_AspNetUsers_PaidBy",
                table: "ProviderPayments");

            migrationBuilder.RenameColumn(
                name: "PaidBy",
                table: "ProviderPayments",
                newName: "PaidByUserId");

            migrationBuilder.RenameIndex(
                name: "IX_ProviderPayments_PaidBy",
                table: "ProviderPayments",
                newName: "IX_ProviderPayments_PaidByUserId");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "ProviderPayments",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Notes",
                table: "ProviderPayments",
                type: "character varying(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "ProviderPayments",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ProviderPayments_AspNetUsers_PaidByUserId",
                table: "ProviderPayments",
                column: "PaidByUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProviderPayments_AspNetUsers_PaidByUserId",
                table: "ProviderPayments");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "ProviderPayments");

            migrationBuilder.DropColumn(
                name: "Notes",
                table: "ProviderPayments");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "ProviderPayments");

            migrationBuilder.RenameColumn(
                name: "PaidByUserId",
                table: "ProviderPayments",
                newName: "PaidBy");

            migrationBuilder.RenameIndex(
                name: "IX_ProviderPayments_PaidByUserId",
                table: "ProviderPayments",
                newName: "IX_ProviderPayments_PaidBy");

            migrationBuilder.AddForeignKey(
                name: "FK_ProviderPayments_AspNetUsers_PaidBy",
                table: "ProviderPayments",
                column: "PaidBy",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
