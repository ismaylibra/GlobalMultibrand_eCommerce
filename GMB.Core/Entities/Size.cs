﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMB.Core.Entities
{
    public class Size : Entity
    {
        public string Name { get; set; }
        public List<ProductSize> ProductSizes { get; set; }

    }
}
