namespace GNB.Infrastructure.Capabilities
{
    public enum ErrorCode
    {
        UnexpectedError = 1000,
        InvalidSku = 1005,
        UnableToRetrieveRatesFromQuietStone = 1010,
        UnableToRetrieveTransactionsFromQuietStone = 1015,
        ExchangeRateNotFound = 1020,
        InvalidCurrency = 1030
    }
}
