using System.Threading.Tasks;
using GNB.Core;
using GNB.Core.Repositories;
using GNB.Core.Traces;
using GNB.Core.UnitOfWork;
using GNB.Data.Repositories;

namespace GNB.Data.UnitOfWork
{
    public class EfUnitOfWork : IUnitOfWork
    {
        private readonly GNBDbContext _context;
        
        private IRepository<Transaction, string> _transactionRepository;
        private IRepository<TransactionTrace, string> _transactionTrace;
        private IRepository<Rate, string> _rateRepository;
        private IRepository<RateTrace, string> _rateTraceRepository;

        public EfUnitOfWork(GNBDbContext context)
        {
            _context = context;
        }

        public async Task Commit()
        {
            await _context.SaveChangesAsync();
        }

        public IRepository<RateTrace, string> RateTraceRepository => _rateTraceRepository ??= new Repository<RateTrace, string>(_context);

        public IRepository<TransactionTrace, string> TransactionTraceRepository => _transactionTrace ??= new Repository<TransactionTrace, string>(_context);

        public IRepository<Transaction, string> TransactionRepository => _transactionRepository ??= new Repository<Transaction, string>(_context);

        public IRepository<Rate, string> RateRepository => _rateRepository ??= new Repository<Rate, string>(_context);
    }
}
