using System;

namespace GNB.Core
{
    public class Rate
    {
        public string ID { get; set; } = Guid.NewGuid().ToString("N");
        public string From { get; set; }
        public string To { get; set; }
        public decimal ChangeRate { get; set; }
    }
}
