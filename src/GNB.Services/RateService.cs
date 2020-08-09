using System;
using System.Collections.Generic;
using System.Linq;
using GNB.Core;
using GNB.Core.UnitOfWork;
using GNB.Services.Dtos;
using Mapster;

namespace GNB.Services
{
    public class RateService : IRateService
    {
        private readonly IUnitOfWork _unitOfWork;

        public RateService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IEnumerable<RateDto> GetRates()
        {
            return _unitOfWork.RateRepository.GetAll().Select(x => x.Adapt<RateDto>());
        }
    }
}
