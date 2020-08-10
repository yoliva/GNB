using System.Reflection;
using GNB.Core;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GNB.Data
{
    public class GNBDbContext : IdentityDbContext
    {
        public GNBDbContext()
        {
        }

        public GNBDbContext(DbContextOptions<GNBDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Transaction> Transactions { get; set; }

        public DbSet<Rate> Rates { get; set; }

        public DbSet<TransactionTrace> TransactionTraces { get; set; }
    }
}
