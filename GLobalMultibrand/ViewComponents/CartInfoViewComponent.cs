using GMB.Business.Helpers;
using GMB.Business.ViewModels.Baskets;
using GMB.DAL.DAL;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace GLobalMultibrand.ViewComponents
{
    public class CartInfoViewComponent : ViewComponent
    {
        private readonly GMBDbContext _dbContext;

        public CartInfoViewComponent(GMBDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            List<BasketItemViewModel> model = new();
            if (Request.Cookies.TryGetValue(Constants.BASKET_COOKIE_NAME, out var cookie))
            {
                var productList = JsonConvert.DeserializeObject<List<BasketItemViewModel>>(cookie);

                foreach (var item in productList)
                {
                    var product = _dbContext.Products
                        .Where(p => p.Id == item.Id && !p.IsDeleted)
                        .FirstOrDefault();
                    var finalPrice = default(decimal);

                    if (product.DiscountPrice > 0)
                    {
                        finalPrice = (decimal)product.Price - (decimal)product.DiscountPrice;
                    }
                    else
                    {
                        finalPrice = (decimal)product.Price;
                    }

                    model.Add(new BasketItemViewModel
                    {
                        Id = product.Id,
                        ProductName = product.ProductName,
                        Price = finalPrice,
                        Count = item.Count,
                        ImageUrl = product.ImageUrl
                    });
                }
            }
            return View(model);
        }
    }
}
