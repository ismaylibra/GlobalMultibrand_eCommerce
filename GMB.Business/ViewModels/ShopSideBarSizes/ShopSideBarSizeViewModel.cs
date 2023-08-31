using GMB.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMB.Business.ViewModels.ShopSideBarSizes
{
    public class ShopSideBarSizeViewModel
    {
        public List<Size> Sizes { get; set; } = new List<Size>();
        public List<Product> Products { get; set; } = new List<Product>();
    }
}
