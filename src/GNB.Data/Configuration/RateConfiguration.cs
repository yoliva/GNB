using GNB.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GNB.Data.Configuration
{
    public class RateConfiguration : IEntityTypeConfiguration<Rate>
    {
        public void Configure(EntityTypeBuilder<Rate> builder)
        {
            builder.HasKey(x => x.ID);

            builder.Property(x => x.From).HasMaxLength(10).IsRequired();
            builder.Property(x => x.To).HasMaxLength(10).IsRequired();
            builder.Property(x => x.ChangeRate).HasColumnType("decimal(12,3)").IsRequired();
        }
    }
}
