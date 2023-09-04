using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMB.Business.ViewModels.Languages
{
    public class LanguageCreateViewModel
    {
        public string Name { get; set; }
        public string IsoCode { get; set; }
        public IFormFile Image { get; set; }
    }
}
    