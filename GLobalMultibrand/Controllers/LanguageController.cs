using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;

namespace GLobalMultibrand.Controllers
{
    public class LanguageController : Controller
    {
        public IActionResult ChangeLanguage(string culture)
        {
            Response.Cookies.Append(CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions() { Expires = DateTimeOffset.UtcNow.AddYears(5) });


            return Redirect(Request.Headers["Referer"].ToString());
        }
    }
}
