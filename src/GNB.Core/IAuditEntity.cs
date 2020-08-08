using System;

namespace GNB.Core
{
    public interface IAuditEntity
    {
        public DateTime CreatedAt { get; }
        public DateTime LastUpdatedAt { get; }
    }
}