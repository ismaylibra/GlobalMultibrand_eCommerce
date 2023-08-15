using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace GLobalMultibrand.Areas.Admin.Controllers
{
    [Area("Admin")]
    //[Authorize(Roles = Constants.AdminRole)]
    public class BaseController : Controller
    {
        
    }
}
