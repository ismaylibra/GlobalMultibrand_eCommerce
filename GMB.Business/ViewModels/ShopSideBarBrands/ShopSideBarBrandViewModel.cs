using GMB.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMB.Business.ViewModels.ShopSideBarBrands
{
    public class ShopSideBarBrandViewModel
    {
        public List<Brand> Brands { get; set; } = new List<Brand>();
        public List<Product> Products { get; set; } = new List<Product>();
    }
}
