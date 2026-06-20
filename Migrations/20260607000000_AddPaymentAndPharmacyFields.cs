using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DiaspoDirect.Migrations
{
    /// <inheritdoc />
    public partial class AddPaymentAndPharmacyFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "RecipientPhone",
                table: "MyPrescriptions",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(30)",
                oldMaxLength: 30,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StripePaymentIntentId",
                table: "MyPrescriptions",
                type: "character varying(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "ServiceFeeUsd",
                table: "MyPrescriptions",
                type: "numeric(10,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "TotalAmountUsd",
                table: "MyPrescriptions",
                type: "numeric(10,2)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "PaidAt",
                table: "MyPrescriptions",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "PharmacyId",
                table: "MyPrescriptions",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_MyPrescriptions_PharmacyId",
                table: "MyPrescriptions",
                column: "PharmacyId");

            migrationBuilder.AddForeignKey(
                name: "FK_MyPrescriptions_Pharmacies_PharmacyId",
                table: "MyPrescriptions",
                column: "PharmacyId",
                principalTable: "Pharmacies",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MyPrescriptions_Pharmacies_PharmacyId",
                table: "MyPrescriptions");

            migrationBuilder.DropIndex(
                name: "IX_MyPrescriptions_PharmacyId",
                table: "MyPrescriptions");

            migrationBuilder.DropColumn(
                name: "StripePaymentIntentId",
                table: "MyPrescriptions");

            migrationBuilder.DropColumn(
                name: "ServiceFeeUsd",
                table: "MyPrescriptions");

            migrationBuilder.DropColumn(
                name: "TotalAmountUsd",
                table: "MyPrescriptions");

            migrationBuilder.DropColumn(
                name: "PaidAt",
                table: "MyPrescriptions");

            migrationBuilder.DropColumn(
                name: "PharmacyId",
                table: "MyPrescriptions");

            migrationBuilder.AlterColumn<string>(
                name: "RecipientPhone",
                table: "MyPrescriptions",
                type: "character varying(30)",
                maxLength: 30,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50,
                oldNullable: true);
        }
    }
}
