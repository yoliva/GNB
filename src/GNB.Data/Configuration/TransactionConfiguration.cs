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

            builder.Ignore(x => x.TableName);

            builder.Property(x => x.Sku).HasMaxLength(50).IsRequired();
            builder.Property(x => x.Amount).HasColumnType("decimal(12,2)");
            builder.Property(x => x.Currency).HasMaxLength(10).IsRequired();

            builder.Property(x => x.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
            builder.Property(x => x.LastUpdatedAt).HasDefaultValueSql("GETUTCDATE()");
        }
    }
}
