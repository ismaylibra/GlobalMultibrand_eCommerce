using GMB.Business.ViewModels.ShopSidebars;
using GMB.DAL.DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GLobalMultibrand.ViewComponents
{
    public class ShopSideBarViewComponent : ViewComponent
    {
        private readonly GMBDbContext _dbContext;

        public ShopSideBarViewComponent(GMBDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            //var categories = await _dbContext.ProductCategories.Where(c => !c.IsDeleted).ToListAsync();
            var brands = await _dbContext.Brands.Where(c => !c.IsDeleted).ToListAsync();
            var colors = await _dbContext.Colors.Where(c => !c.IsDeleted).ToListAsync();
            var sizes = await _dbContext.Sizes.Where(c => !c.IsDeleted).ToListAsync();


            var viewModel = new ShopSideBarViewModel
            {
                //Categories = categories,
                Brands = brands,
                Colors = colors,
                Sizes = sizes
            };
            return View(viewModel);
        }
    }
}

