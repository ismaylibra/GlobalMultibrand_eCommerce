using GMB.Core.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMB.Business.ViewModels.Products
{
    public class ProductUpdateViewModel
    {
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public decimal? DiscountPrice { get; set; }
        public string ProductShortDescription { get; set; }
        public string ProductFullDescription { get; set; }
        public List<SelectListItem>? Brands { get; set; }
        public int BrandId { get; set; }
        public string? ImageUrl { get; set; }
        public IFormFile? Image { get; set; }
        public string? RemovedImageIds { get; set; }
        public IFormFile[] Images { get; set; }
        public List<ProductImage>? ProductImages { get; set; }
        //public List<SelectListItem>? Categories { get; set; }
        //public List<int> CategoryIds { get; set; }
        public List<SelectListItem>? Colors { get; set; }
        public List<int> ColorIds { get; set; }
        public List<SelectListItem>? Sizes { get; set; }
        public List<int> SizeIds { get; set; }
    }
}
