namespace GNB.Services.Dtos
{
    public class TransactionDto
    {
        public string ID { get; set; }
        public string Sku { get; set; }
        public string Currency { get; set; }
        public decimal Amount { get; set; }
    }
}
