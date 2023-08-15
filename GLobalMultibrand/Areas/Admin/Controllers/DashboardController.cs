using Microsoft.AspNetCore.Mvc;

namespace GLobalMultibrand.Areas.Admin.Controllers
{
    public class DashboardController : BaseController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
