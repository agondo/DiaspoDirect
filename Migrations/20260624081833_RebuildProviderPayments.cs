using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DiaspoDirect.Migrations
{
    /// <inheritdoc />
    public partial class RebuildProviderPayments : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PaymentDate",
                table: "ProviderPayments");

            migrationBuilder.RenameColumn(
                name: "PrescriptionId",
                table: "ProviderPayments",
                newName: "PaymentId");

            migrationBuilder.RenameColumn(
                name: "AmountCFA",
                table: "ProviderPayments",
                newName: "AmountXOF");

            migrationBuilder.RenameColumn(
                name: "ProviderPaymentId",
                table: "ProviderPayments",
                newName: "Id");

            migrationBuilder.AlterColumn<string>(
                name: "PaymentMethod",
                table: "ProviderPayments",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "Wave",
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "PaidAt",
                table: "ProviderPayments",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PaidBy",
                table: "ProviderPayments",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ReceiptUrl",
                table: "ProviderPayments",
                type: "character varying(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProviderPayments_PaidBy",
                table: "ProviderPayments",
                column: "PaidBy");

            migrationBuilder.CreateIndex(
                name: "IX_ProviderPayments_PaymentId",
                table: "ProviderPayments",
                column: "PaymentId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProviderPayments_AspNetUsers_PaidBy",
                table: "ProviderPayments",
                column: "PaidBy",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_ProviderPayments_Payments_PaymentId",
                table: "ProviderPayments",
                column: "PaymentId",
                principalTable: "Payments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProviderPayments_AspNetUsers_PaidBy",
                table: "ProviderPayments");

            migrationBuilder.DropForeignKey(
                name: "FK_ProviderPayments_Payments_PaymentId",
                table: "ProviderPayments");

            migrationBuilder.DropIndex(
                name: "IX_ProviderPayments_PaidBy",
                table: "ProviderPayments");

            migrationBuilder.DropIndex(
                name: "IX_ProviderPayments_PaymentId",
                table: "ProviderPayments");

            migrationBuilder.DropColumn(
                name: "PaidAt",
                table: "ProviderPayments");

            migrationBuilder.DropColumn(
                name: "PaidBy",
                table: "ProviderPayments");

            migrationBuilder.DropColumn(
                name: "ReceiptUrl",
                table: "ProviderPayments");

            migrationBuilder.RenameColumn(
                name: "PaymentId",
                table: "ProviderPayments",
                newName: "PrescriptionId");

            migrationBuilder.RenameColumn(
                name: "AmountXOF",
                table: "ProviderPayments",
                newName: "AmountCFA");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "ProviderPayments",
                newName: "ProviderPaymentId");

            migrationBuilder.AlterColumn<string>(
                name: "PaymentMethod",
                table: "ProviderPayments",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50);

            migrationBuilder.AddColumn<DateTime>(
                name: "PaymentDate",
                table: "ProviderPayments",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
