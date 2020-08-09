using System;
using System.Collections.Generic;
using System.Linq;
using GNB.Core.UnitOfWork;
using GNB.Services.Dtos;
using Mapster;

namespace GNB.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly IUnitOfWork _unitOfWork;

        public TransactionService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IEnumerable<TransactionDto> GetTransactions()
        {
            return _unitOfWork.TransactionRepository.GetAll()
                .Select(x => x.Adapt<TransactionDto>())
                .AsEnumerable();
        }

        public IEnumerable<TransactionDto> GetTransactionsBySku(string sku)
        {
            if (string.IsNullOrEmpty(sku))
                throw new ArgumentNullException(nameof(sku));

            return _unitOfWork.TransactionRepository.GetAll()
                .Where(x => x.Sku == sku)
                .Select(x => x.Adapt<TransactionDto>())
                .AsEnumerable();
        }
    }
}
