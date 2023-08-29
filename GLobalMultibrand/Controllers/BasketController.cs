using GMB.Business.Helpers;
using GMB.Business.ViewModels.Baskets;
using GMB.DAL.DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace GLobalMultibrand.Controllers
{
    public class BasketController : Controller
    {
        private readonly GMBDbContext _dbContext;

        public BasketController(GMBDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IActionResult> Index()
        {
            var basketItems = Request.Cookies["basket"];

            if (string.IsNullOrEmpty(basketItems))
            {
                return View(new List<BasketItemViewModel>());
            }

            return View(JsonConvert.DeserializeObject<List<BasketItemViewModel>>(basketItems));
        }

        public async Task<IActionResult> AddToBasket(int? productId)
        {
            if (productId is null) return BadRequest();
            var product = await _dbContext.Products
                .Where(p => p.Id == productId)
                .FirstOrDefaultAsync();

            if (product is null) return NotFound();

            var basket = Request.Cookies["basket"];
            var basketItems = new List<BasketItemViewModel>();
            var basketItem = new BasketItemViewModel
            {
                Id = product.Id,
                ProductName = product.ProductName,
                Price = product.Price,
                DiscountPrice = product.DiscountPrice,
                ImageUrl = product.ImageUrl,
                Count = 1

            };

            if (basket is null)
            {
              

                basketItems.Add(basketItem);
            }
            else
            {

                basketItems = JsonConvert.DeserializeObject<List<BasketItemViewModel>>(basket);


                var existProduct =  basketItems.Where(b => b.Id == product.Id).FirstOrDefault();

                if (existProduct is null)
                {

                basketItems.Add(basketItem);
                }
                else
                {
                    existProduct.Count += 1;
                }



            }

            Response.Cookies.Append("basket", JsonConvert.SerializeObject(basketItems));

            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> UpdateBasketQuantity(int? productId, int updatedQuantity)
        {
            if (productId == null || updatedQuantity < 0)
            {
                return BadRequest("Invalid arguments");
            }

            var basket = Request.Cookies["basket"];
            if (basket is null)
            {
                return NotFound("Basket cookie not found");
            }

            var basketItems = JsonConvert.DeserializeObject<List<BasketItemViewModel>>(basket);
            var productToUpdate = basketItems.Where(x => x.Id == productId).FirstOrDefault();

            if (productToUpdate == null)
            {
                return NotFound("Product not found in basket");
            }


            productToUpdate.Count = updatedQuantity;


            Response.Cookies.Append("basket", JsonConvert.SerializeObject(basketItems));

            return Ok();
        }


        [HttpPost]
        public async Task<IActionResult> DeleteProductBasket(int? productId)
        {
            if (Request.Cookies.TryGetValue(Constants.BASKET_COOKIE_NAME, out var cookie))
            {
                var productList = JsonConvert.DeserializeObject<List<BasketItemViewModel>>(cookie);

                var existProduct = productList.Where(x => x.Id == productId).FirstOrDefault();

                productList.Remove(existProduct);

                var productIdListJson = JsonConvert.SerializeObject(productList);

                Response.Cookies.Append(Constants.BASKET_COOKIE_NAME, productIdListJson);
            }
            return NoContent();
        }
    }
}
