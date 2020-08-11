using System;

namespace GNB.Core.Traces
{
    public class TransactionTrace : IAuditEntity, IEntity
    {
        public string ID { get; set; } = Guid.NewGuid().ToString("N");
        public string TransactionList { get; set; }
        public TraceStatus Status { get; set; }
        public DateTime CreatedAt { get; }
        public DateTime LastUpdatedAt { get; }
        public string TableName => "TransactionTraces";
    }
}
