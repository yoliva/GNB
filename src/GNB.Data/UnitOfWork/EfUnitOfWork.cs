using GNB.Core;
using GNB.Core.Repositories;
using GNB.Core.UnitOfWork;
using GNB.Data.Repositories;

namespace GNB.Data.UnitOfWork
{
    public class EfUnitOfWork : IUnitOfWork
    {
        private readonly GNBDbContext _context;
        
        private IRepository<Transaction, string> _transactionRepository;
        private IRepository<Rate, string> _rateRepository;

        public EfUnitOfWork(GNBDbContext context)
        {
            _context = context;
        }

        public void Commit()
        {
            _context.SaveChanges();
        }

        public IRepository<TransactionTrace, string> TransactionTraceRepository { get; }

        public IRepository<Transaction, string> TransactionRepository => _transactionRepository ??= new Repository<Transaction, string>(_context);

        public IRepository<Rate, string> RateRepository => _rateRepository ??= new Repository<Rate, string>(_context);
    }
}
