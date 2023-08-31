using GMB.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMB.Business.ViewModels.ShopSideBarColors
{
    public class ShopSideBarColorViewModel
    {
        public List<Color> Colors { get; set; } = new List<Color>();
        public List<Product> Products { get; set; } = new List<Product>();
    }
}
