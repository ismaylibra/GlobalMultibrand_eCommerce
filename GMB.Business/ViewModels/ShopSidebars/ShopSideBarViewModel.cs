using GMB.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMB.Business.ViewModels.ShopSidebars
{
    public class ShopSideBarViewModel
    {
        public List<Product> Products { get; set; } = new List<Product>();
        //public List<ProductCategory> Categories { get; set; } = new List<ProductCategory>();
        public List<Brand> Brands { get; set; } = new List<Brand>();
        public List<Color> Colors { get; set; } = new List<Color>();
        public List<Size> Sizes { get; set; } = new List<Size>();


    }
}
