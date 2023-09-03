using GMB.DAL.DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GLobalMultibrand.ViewComponents
{
    public class ProductsSliderViewComponent : ViewComponent
    {
        private readonly GMBDbContext _dbContext;

        public ProductsSliderViewComponent(GMBDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var product = await _dbContext.Products.Where(p => !p.IsDeleted)
                .Include(p => p.ProductImages)
                //.Include(p => p.CategoryProducts).ThenInclude(p => p.Category)
                .Include(p => p.Brand)
                .ToListAsync();
            return View(product);
        }
    }
}
