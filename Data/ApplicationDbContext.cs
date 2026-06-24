using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DiaspoDirect.Data
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser>(options)
    {
        public DbSet<Pharmacy> Pharmacies { get; set; }
        public DbSet<MyPrescription> MyPrescriptions { get; set; }
        public DbSet<SendPrescription> SendPrescriptions { get; set; }
        public DbSet<ExchangeRate> ExchangeRates { get; set; }
        public DbSet<Transactions> Transactions { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Provider> Providers { get; set; }
        public DbSet<ProviderPayment> ProviderPayments { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<ProviderPayment>(entity =>
            {
                entity.HasOne(pp => pp.Payment)
                      .WithMany()
                      .HasForeignKey(pp => pp.PaymentId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(pp => pp.PaidByUser)
                      .WithMany()
                      .HasForeignKey(pp => pp.PaidBy)
                      .OnDelete(DeleteBehavior.SetNull);

                entity.Property(pp => pp.PaymentMethod)
                      .HasConversion<string>()
                      .HasMaxLength(50);

                entity.Property(pp => pp.Status)
                      .HasConversion<string>()
                      .HasMaxLength(50);
            });
        }
    }
}
