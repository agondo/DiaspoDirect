using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DiaspoDirect.Migrations
{
    /// <inheritdoc />
    public partial class AddProviderToSendPrescription : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ProviderId",
                table: "SendPrescriptions",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SendPrescriptions_ProviderId",
                table: "SendPrescriptions",
                column: "ProviderId");

            migrationBuilder.AddForeignKey(
                name: "FK_SendPrescriptions_Providers_ProviderId",
                table: "SendPrescriptions",
                column: "ProviderId",
                principalTable: "Providers",
                principalColumn: "ProviderId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SendPrescriptions_Providers_ProviderId",
                table: "SendPrescriptions");

            migrationBuilder.DropIndex(
                name: "IX_SendPrescriptions_ProviderId",
                table: "SendPrescriptions");

            migrationBuilder.DropColumn(
                name: "ProviderId",
                table: "SendPrescriptions");
        }
    }
}
