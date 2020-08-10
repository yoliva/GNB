using GNB.Core.Repositories;
using GNB.Core.Traces;

namespace GNB.Core.UnitOfWork
{
    public interface IUnitOfWork
    {
        IRepository<Transaction, string> TransactionRepository { get; }
        IRepository<Rate, string> RateRepository { get; }
        IRepository<TransactionTrace, string> TransactionTraceRepository { get; }
        IRepository<RateTrace, string> RateTraceRepository { get; }

        void Commit();
    }
}
