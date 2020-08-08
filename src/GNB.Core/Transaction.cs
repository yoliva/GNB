using System;

namespace GNB.Core
{
    public class Transaction
    {
        public string ID { get; set; } = Guid.NewGuid().ToString("N");
        public string Sku { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; }
    }
}
