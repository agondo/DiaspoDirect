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
    }
}
