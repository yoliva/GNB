using GNB.Core.Repositories;

namespace GNB.Core.UnitOfWork
{
    public interface IUnitOfWork
    {
        IRepository<Transaction, string> TransactionRepository { get; }
        IRepository<Rate, string> RateRepository { get; }
        IRepository<TransactionTrace, string> TransactionTraceRepository { get; }

        void Commit();
    }
}
