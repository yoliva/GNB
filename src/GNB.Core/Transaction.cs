using System;

namespace GNB.Core
{
    public class Transaction : IAuditEntity
    {
        public string ID { get; set; } = Guid.NewGuid().ToString("N");
        public string Sku { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; }
        public DateTime CreatedAt { get; }
        public DateTime LastUpdatedAt { get; }
    }
}
