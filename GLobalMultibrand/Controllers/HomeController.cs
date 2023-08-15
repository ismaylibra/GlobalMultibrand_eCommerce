using GMB.Business.ViewModels;
using GMB.DAL.DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Versioning;

namespace GLobalMultibrand.Controllers
{
    public class HomeController : Controller
    {
        private readonly GMBDbContext _dbContext;

        public HomeController(GMBDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IActionResult> Index()
        {
            var sliders = await _dbContext.Sliders
               .Where(s => !s.IsDeleted)
               .OrderByDescending(s => s.Id)
               .ToListAsync();

            var homeVM = new HomeViewModel
            {
                Sliders = sliders
            };

            return View(homeVM);
        }
    }
}
