using System.Threading.Tasks;
using GNB.Core.UnitOfWork;
using Microsoft.AspNetCore.Mvc.Filters;

namespace GNB.Api.Filters
{
    public class UnitOfWorkCommitChangesFilter : IAsyncActionFilter
    {
        private readonly IUnitOfWork _unitOfWork;

        public UnitOfWorkCommitChangesFilter(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var result = await next();
            if (result.Exception == null || result.ExceptionHandled)
            {
                await _unitOfWork.Commit();
            }
        }
    }
}
