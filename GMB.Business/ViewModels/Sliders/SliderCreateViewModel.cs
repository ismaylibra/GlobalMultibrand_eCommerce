using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMB.Business.ViewModels.Sliders
{
    public class SliderCreateViewModel
    {
        public string? ImageUrl { get; set; }
        public string Title { get; set; }
        public string Subtitle { get; set; }
        public IFormFile Image { get; set; }
        public string Slogan { get; set; }
    }
}
