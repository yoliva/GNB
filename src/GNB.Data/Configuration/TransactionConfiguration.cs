using GNB.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GNB.Data.Configuration
{
    public class TransactionConfiguration : IEntityTypeConfiguration<Transaction>
    {
        public void Configure(EntityTypeBuilder<Transaction> builder)
        {
            builder.HasKey(x => x.ID);

            builder.Property(x => x.Sku).HasMaxLength(100).IsRequired();
            builder.Property(x => x.Amount).HasColumnType("decimal(12,2)");
            builder.Property(x => x.Currency).HasMaxLength(100).IsRequired();
        }
    }
}
