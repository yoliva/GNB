using System;

namespace GNB.Core
{
    public enum TraceStatus
    {
        Pending,
        Processed
    }

    public class TransactionTrace : IAuditEntity
    {
        public string ID { get; set; } = Guid.NewGuid().ToString("N");
        public string TransactionList { get; set; }
        public TraceStatus Status { get; set; }
        public DateTime CreatedAt { get; }
        public DateTime LastUpdatedAt { get; }
    }
}
