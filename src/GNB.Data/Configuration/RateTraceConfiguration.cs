using GNB.Core.Traces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GNB.Data.Configuration
{
    public class RateTraceConfiguration : IEntityTypeConfiguration<RateTrace>
    {
        public void Configure(EntityTypeBuilder<RateTrace> builder)
        {
            builder.HasKey(x => x.ID);

            builder.Ignore(x => x.TableName);

            builder.Property(x => x.RateList).IsRequired();

            builder.Property(x => x.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
            builder.Property(x => x.LastUpdatedAt).HasDefaultValueSql("GETUTCDATE()");
        }
    }
}
