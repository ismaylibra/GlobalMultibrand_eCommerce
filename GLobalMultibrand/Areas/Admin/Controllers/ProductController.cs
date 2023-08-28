using GMB.Business.Helpers;
using GMB.Business.ViewModels.Products;
using GMB.Core.Entities;
using GMB.DAL.DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Watch.BLL.Extensions;

namespace GLobalMultibrand.Areas.Admin.Controllers
{
    public class ProductController : BaseController
    {
        private readonly GMBDbContext _dbContext;

        public ProductController(GMBDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IActionResult> Index()
        {
            var products = await _dbContext.Products.Where(p => !p.IsDeleted).Include(x => x.ProductImages).Include(p => p.Brand).OrderByDescending(p => p.Id).ToListAsync();
            return View(products);
        }
        public async Task<IActionResult> Create()
        {
            var brands = await _dbContext.Brands.Where(c => !c.IsDeleted).ToListAsync();
            var colors = await _dbContext.Colors.Where(c => !c.IsDeleted).ToListAsync();
            var sizes = await _dbContext.Sizes.Where(c => !c.IsDeleted).ToListAsync();

            //var categories = await _dbContext.ProductCategories.Where(c => !c.IsDeleted).ToListAsync();

            var brandListItem = new List<SelectListItem>
            {
                new SelectListItem("Select Brand Of Product" , "0",false)
            };
            //var categoryListItem = new List<SelectListItem>
            //{
            //    new SelectListItem("Select Category Of Product" , "0",false)
            //};

            var colorListItem = new List<SelectListItem>
            {
                new SelectListItem("Select Color Of Product" , "0", false)
            };

            var sizeListItem = new List<SelectListItem>
            {
                new SelectListItem("Select Size Of Product" , "0", false)
            };

            brands.ForEach(c => brandListItem.Add(new SelectListItem(c.Name, c.Id.ToString())));
            colors.ForEach(c => colorListItem.Add(new SelectListItem(c.Name, c.Id.ToString())));
            sizes.ForEach(c => sizeListItem.Add(new SelectListItem(c.Name, c.Id.ToString())));

            //categories.ForEach(c => categoryListItem.Add(new SelectListItem(c.Name, c.Id.ToString())));

            var model = new ProductCreateViewModel
            {
                Brands = brandListItem,
                Colors = colorListItem,
                Sizes = sizeListItem
                //Categories = categoryListItem
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductCreateViewModel model)
        {

            var createdProduct = new Product
            {
                ProductName = model.ProductName,
                Price = model.Price,
                DiscountPrice = model.DiscountPrice,
                ProductFullDescription = model.ProductFullDescription,
                ProductShortDescription = model.ProductShortDescription,
                BrandId = model.BrandId,
                ProductImages = new List<ProductImage>(),
            };


            if (!ModelState.IsValid) return View(model);
            var productImage = new List<ProductImage>();
            foreach (var item in model.ProductImages)
            {
                if (!item.IsImage())
                {
                    ModelState.AddModelError("Image", "Image must be selected..!");
                    return View(model);
                }

                if (!item.IsAllowedSize(20))
                {
                    ModelState.AddModelError("Image", "Image size can be maximum 20mb..!");
                    return View(model);
                }
                var unicalFileName = await item.GenerateFile(Constants.ProductImagePath);
                productImage.Add(new ProductImage
                {
                    Name = unicalFileName,
                    ProductId = createdProduct.Id
                });
            }

            createdProduct.ProductImages.AddRange(productImage);

            if (!model.Image.IsImage())
            {
                ModelState.AddModelError("Image", " An Image Must be Selected..!");
                return View(model);
            }

            if (!model.Image.IsAllowedSize(20))
            {
                ModelState.AddModelError("Image", "Image Size Can be Maximum 20mb..!");
                return View(model);
            }

            var unicalFileName1 = await model.Image.GenerateFile(Constants.ProductMainImagePath);

            createdProduct.ImageUrl = unicalFileName1;

            List<ProductColor> productColors = new List<ProductColor>();

            foreach (var colorId in model.ColorIds)
            {
                if (!await _dbContext.Colors.AnyAsync(c => c.Id == colorId))
                {
                    ModelState.AddModelError("", "There was no such color..!");
                }
                productColors.Add(new ProductColor
                {
                    ColorId = colorId
                });
            }
            var colors = await _dbContext.Colors.Where(s => !s.IsDeleted).ToListAsync();
            var colorSelectListItem = new List<SelectListItem>();
            colors.ForEach(c => colorSelectListItem.Add(new SelectListItem(c.Name, c.Id.ToString())));
            createdProduct.ProductColors = productColors;
            model.Colors = colorSelectListItem;


            List<ProductSize> productSizes = new List<ProductSize>();

            foreach (var sizeId in model.SizeIds)
            {
                if (!await _dbContext.Sizes.AnyAsync(s => s.Id == sizeId))
                {
                    ModelState.AddModelError("", "There was no such size..!");
                }
                productSizes.Add(new ProductSize
                {
                    SizeId = sizeId
                });
            }
            var sizes = await _dbContext.Sizes.Where(s => !s.IsDeleted).ToListAsync();
            var sizeselectListItems = new List<SelectListItem>();
            sizes.ForEach(c => sizeselectListItems.Add(new SelectListItem(c.Name, c.Id.ToString())));
            createdProduct.ProductSizes = productSizes;
            model.Sizes = sizeselectListItems;



            //List<CategoryProduct> categoryProducts = new List<CategoryProduct>();

            //foreach (var categoryId in model.CategoryIds)
            //{
            //    if (!await _dbContext.ProductCategories.AnyAsync(c => c.Id == categoryId))
            //    {
            //        ModelState.AddModelError("", "There was no such category..!");
            //    }
            //    categoryProducts.Add(new CategoryProduct
            //    {
            //        CategoryId = categoryId
            //    });
            //}

            //var categories = await _dbContext.ProductCategories.Where(s => !s.IsDeleted).ToListAsync();
            //var categoriesSelectListItem = new List<SelectListItem>();
            //categories.ForEach(c => categoriesSelectListItem.Add(new SelectListItem(c.Name, c.Id.ToString())));
            //createdProduct.CategoryProducts = categoryProducts;
            //model.Categories = categoriesSelectListItem;


            var brands = await _dbContext.Brands.Where(c => !c.IsDeleted).Include(c => c.Products).ToListAsync();
            if (!ModelState.IsValid) return View(model);

            var brandListItem = new List<SelectListItem>
             {
                 new SelectListItem("Select Brand Of Product", "0")
             };

            brands.ForEach(c => brandListItem.Add(new SelectListItem(c.Name, c.Id.ToString())));

            var viewModel = new ProductCreateViewModel
            {
                Brands = brandListItem



            };


            await _dbContext.Products.AddAsync(createdProduct);
            await _dbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Update(int? id)
        {
            if (id is null) return BadRequest();
            var product = await _dbContext.Products
                .Where(p => p.Id == id)
                .Include(p => p.ProductImages)
                //.Include(p => p.CategoryProducts)
                //.ThenInclude(c => c.Category)
                .Include(p => p.ProductColors)
                .ThenInclude(c => c.Color)
                .Include(p => p.ProductSizes)
                .ThenInclude(c => c.Size)
                .Include(p => p.Brand)
                .FirstOrDefaultAsync();

            if (product is null) return NotFound();

            if (product.Id != id) return BadRequest();

            //var categories = await _dbContext.ProductCategories
            //    .Where(c => !c.IsDeleted)
            //    .ToListAsync();

            //if (categories is null) return NotFound();

            var brands = await _dbContext.Brands
                .Where(b => !b.IsDeleted)
                .ToListAsync();

            if (brands is null) return NotFound();

            var colors = await _dbContext.Colors
                .Where(c => !c.IsDeleted)
                .ToListAsync();

            if (colors is null) return NotFound();

            var sizes = await _dbContext.Sizes
                .Where(c => !c.IsDeleted)
                .ToListAsync();

            if (sizes is null) return NotFound();


            //var selectedCategories = new List<SelectListItem>();

            //categories.ForEach(c => selectedCategories.Add(new SelectListItem(c.Name, c.Id.ToString())));

            var selectedBrands = new List<SelectListItem>();

            brands.ForEach(c => selectedBrands.Add(new SelectListItem(c.Name, c.Id.ToString())));

            var selectedColors = new List<SelectListItem>();

            colors.ForEach(c => selectedColors.Add(new SelectListItem(c.Name, c.Id.ToString())));

            var selectedSizes = new List<SelectListItem>();

            sizes.ForEach(c => selectedSizes.Add(new SelectListItem(c.Name, c.Id.ToString())));

            var productUpdateViewModel = new ProductUpdateViewModel
            {
                ProductName = product.ProductName,
                ProductFullDescription = product.ProductFullDescription,
                ProductShortDescription = product.ProductShortDescription,
                Price = product.Price,
                Brands = selectedBrands,
                //Categories = selectedCategories,
                Colors = selectedColors,
                Sizes=selectedSizes,
                ProductImages = product.ProductImages,
                ImageUrl = product.ImageUrl

            };

            return View(productUpdateViewModel);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int? id, ProductUpdateViewModel model)
        {

            if (id is null) return BadRequest();

            var product = await _dbContext.Products
                .Where(p => p.Id == id)
                .Include(p => p.ProductImages)
                //.Include(p => p.CategoryProducts).ThenInclude(p => p.Category)
                .Include(p => p.ProductColors).ThenInclude(p => p.Color)
                .Include(p => p.ProductSizes).ThenInclude(p => p.Size)
                .Include(p => p.Brand)
                .FirstOrDefaultAsync();

            if (product is null) return NotFound();

            //var categories = await _dbContext.ProductCategories
            // .Where(c => !c.IsDeleted)
            // .ToListAsync();
            //if (categories is null) return NotFound();

            var brands = await _dbContext.Brands
                .Where(b => !b.IsDeleted)
                .ToListAsync();
            if (brands is null) return NotFound();
            var colors = await _dbContext.Colors
                .Where(c => !c.IsDeleted)
                .ToListAsync();
            if (colors is null) return NotFound();
            var sizes = await _dbContext.Sizes
                .Where(c => !c.IsDeleted)
                .ToListAsync();
            if (sizes is null) return NotFound();

            //var selectedCategories = new List<SelectListItem>();

            //categories.ForEach(c => selectedCategories.Add(new SelectListItem(c.Name, c.Id.ToString())));

            var selectedBrands = new List<SelectListItem>();

            brands.ForEach(c => selectedBrands.Add(new SelectListItem(c.Name, c.Id.ToString())));

            var selectedColors = new List<SelectListItem>();

            colors.ForEach(c => selectedColors.Add(new SelectListItem(c.Name, c.Id.ToString())));

            var selectedSizes = new List<SelectListItem>();

            sizes.ForEach(c => selectedSizes.Add(new SelectListItem(c.Name, c.Id.ToString())));

            model.Brands = selectedBrands;
            model.Colors = selectedColors;
            model.Sizes = selectedSizes;
            //model.Categories = selectedCategories;
            model.ProductImages = product.ProductImages;
            model.DiscountPrice = product.DiscountPrice;

            if (!ModelState.IsValid) return View(model);


            if (model.Image is not null)
            {
                if (!model.Image.IsImage())
                {
                    ModelState.AddModelError("", "Must be selected image");
                    return View(model);
                }

                if (!model.Image.IsAllowedSize(8))
                {
                    ModelState.AddModelError("", "Image size can be max 8 mb");
                    return View(model);
                }

                var productPath = Path.Combine(Constants.RootPath, "assets", "images", "customproduct", product.ImageUrl);

                if (System.IO.File.Exists(productPath))
                    System.IO.File.Delete(productPath);

                var unicalName = await model.Image.GenerateFile(Constants.ProductMainImagePath);

                product.ImageUrl = unicalName;
            }

            List<ProductImage> productImages = new List<ProductImage>();

            if (model.Images is not null)
            {
                foreach (var image in model.Images)
                {
                    if (!image.IsImage())
                    {
                        ModelState.AddModelError("", "Must be selected image");
                        return View(model);
                    }

                    if (!image.IsAllowedSize(8))
                    {
                        ModelState.AddModelError("", "Image size can be max 8 mb");
                        return View(model);
                    }


                    var unicalName = await image.GenerateFile(Constants.ProductImagePath);

                    productImages.Add(new ProductImage
                    {
                        Name = unicalName,
                        ProductId = product.Id,
                    });
                }
            }

            product.ProductImages.AddRange(productImages);

            //if (model.RemoveImageIds is not null)
            //{
            //    RemoveImageIds(model.RemoveImageIds);
            //}

            //List<CategoryProduct> categoryProducts = new List<CategoryProduct>();
            //if (model.CategoryIds.Count > 0)
            //{
            //    foreach (int categoryId in model.CategoryIds)
            //    {
            //        if (!await _dbContext.ProductCategories.AnyAsync(c => c.Id == categoryId))
            //        {
            //            ModelState.AddModelError("", "You chose the wrong category..!");
            //            return View(model);
            //        }
            //        categoryProducts.Add(new CategoryProduct
            //        {
            //            CategoryId = categoryId

            //        });
            //    }
            //    product.CategoryProducts = categoryProducts;

            //}
            //else
            //{
            //    ModelState.AddModelError("", "At least 1 category must be elected..!");
            //    return View(model);
            //}

            //List<ProductColor> productColors = new List<ProductColor>();
            //if (model.ColorIds.Count > 0)
            //{
            //    foreach (int colorId in model.ColorIds)
            //    {
            //        if (!await _dbContext.Colors.AnyAsync(c => c.Id == colorId))
            //        {
            //            ModelState.AddModelError("", "You chose the wrong color..!");
            //            return View(model);
            //        }
            //        productColors.Add(new ProductColor
            //        {
            //            ColorId = colorId
            //        });
            //    }
            //    product.ProductColors = productColors;

            //}
            //else
            //{
            //    ModelState.AddModelError("", "At least 1 color must be elected..!");
            //    return View(model);
            //}






            product.ProductName = model.ProductName;
            product.ProductShortDescription = model.ProductShortDescription;
            product.ProductFullDescription = model.ProductFullDescription;
            product.Price = model.Price;
            product.BrandId = model.BrandId;



            await _dbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));


        }


        public async Task<IActionResult> Delete(int? id)
        {
            if (id is null) return NotFound();

            var existedProduct = await _dbContext.Products.Where(p => p.Id == id).Include(p => p.ProductImages).FirstOrDefaultAsync();
            if (existedProduct is null) return NotFound();
            if (existedProduct.Id != id) return NotFound();
            foreach (var item in existedProduct.ProductImages)
            {
                var eventImage = Path.Combine(Constants.RootPath, "assets", "images", "product", item.Name);
                if (System.IO.File.Exists(eventImage))
                    System.IO.File.Delete(eventImage);
            }

            var path = Path.Combine(Constants.RootPath, "assets", "images", "product", existedProduct.ImageUrl);

            var result = System.IO.File.Exists(path);
            if (result)
            {
                System.IO.File.Delete(path);
            }

            _dbContext.Products.Remove(existedProduct);
            await _dbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }




    }
}
