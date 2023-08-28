using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMB.Core.Entities
{
    public class Product : Entity
    {
        public string ProductName { get; set; }
        public string ImageUrl { get; set; }
        public decimal Price { get; set; }
        public int? DiscountPrice { get; set; }
        public string? ProductShortDescription { get; set; }
        public string ProductFullDescription { get; set; }
        public int BrandId { get; set; }
        public Brand Brand { get; set; }
        public List<ProductImage> ProductImages { get; set; }
        public List<ProductColor> ProductColors { get; set; }
        public List<ProductSize> ProductSizes { get; set; }




    }
}

