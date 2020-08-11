using GNB.Api.Models;
using Microsoft.AspNetCore.Mvc;

namespace GNB.Api.Controllers
{
    public class ErrorsController : Controller
    {
        public IActionResult Index(string id)
        {
            var vm = new ErrorViewModel { ErrorCode = id };

            return View(vm);
        }
    }
}
