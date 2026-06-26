using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DiaspoDirect.Migrations
{
    /// <inheritdoc />
    public partial class CleanPaymentEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsPaidToProvider",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "PaidToProviderAt",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "ProviderPaymentReference",
                table: "Payments");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsPaidToProvider",
                table: "Payments",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "PaidToProviderAt",
                table: "Payments",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProviderPaymentReference",
                table: "Payments",
                type: "text",
                nullable: true);
        }
    }
}
