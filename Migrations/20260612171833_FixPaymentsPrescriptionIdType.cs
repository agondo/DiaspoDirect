using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DiaspoDirect.Migrations
{
    /// <inheritdoc />
    public partial class FixPaymentsPrescriptionIdType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Drop FK and index before altering the column type
            migrationBuilder.Sql(@"
                ALTER TABLE ""Payments"" DROP CONSTRAINT IF EXISTS ""FK_Payments_SendPrescriptions_PrescriptionId"";
                DROP INDEX IF EXISTS ""IX_Payments_PrescriptionId"";
            ");

            // Re-create column as uuid (drop + add since integer→uuid has no implicit cast)
            migrationBuilder.Sql(@"
                ALTER TABLE ""Payments"" DROP COLUMN ""PrescriptionId"";
                ALTER TABLE ""Payments"" ADD COLUMN ""PrescriptionId"" uuid NOT NULL DEFAULT '00000000-0000-0000-0000-000000000000';
            ");

            // Restore index and FK
            migrationBuilder.Sql(@"
                CREATE INDEX ""IX_Payments_PrescriptionId"" ON ""Payments"" (""PrescriptionId"");
                ALTER TABLE ""Payments"" ADD CONSTRAINT ""FK_Payments_SendPrescriptions_PrescriptionId""
                    FOREIGN KEY (""PrescriptionId"") REFERENCES ""SendPrescriptions""(""Id"") ON DELETE CASCADE;
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                ALTER TABLE ""Payments"" DROP CONSTRAINT IF EXISTS ""FK_Payments_SendPrescriptions_PrescriptionId"";
                DROP INDEX IF EXISTS ""IX_Payments_PrescriptionId"";
                ALTER TABLE ""Payments"" DROP COLUMN ""PrescriptionId"";
                ALTER TABLE ""Payments"" ADD COLUMN ""PrescriptionId"" integer NOT NULL DEFAULT 0;
                CREATE INDEX ""IX_Payments_PrescriptionId"" ON ""Payments"" (""PrescriptionId"");
            ");
        }
    }
}
