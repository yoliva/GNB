using Hangfire;

namespace GNB.Jobs
{
    [AutomaticRetry(Attempts = 0)]
    public interface ITransactionImporter
    {
        void Import();
    }
}
