using Hangfire;

namespace GNB.Jobs
{
    public interface IRatesImporter
    {
        [AutomaticRetry(Attempts = 0)]
        void Import();
    }
}
