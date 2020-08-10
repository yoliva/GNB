using System;

namespace GNB.Core.Traces
{
    public class RateTrace : IAuditEntity
    {
        public string ID { get; set; } = Guid.NewGuid().ToString("N");
        public string RateList { get; set; }
        public TraceStatus Status { get; set; }
        public DateTime CreatedAt { get; }
        public DateTime LastUpdatedAt { get; }
    }
}
