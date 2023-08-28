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

                 var products = await _dbContext.Products
                //.Where(p => !p.IsDeleted)
                .Include(p => p.ProductImages)
                //.Include(p => p.CategoryProducts).ThenInclude(p => p.Category)
                .Include(p => p.Brand)
                .Take(6)
                .OrderByDescending(p => p.Id)
                .ToListAsync();

            var homeVM = new HomeViewModel
            {
                Sliders = sliders,
                Products=products
            };

            return View(homeVM);
        }
    }
}
