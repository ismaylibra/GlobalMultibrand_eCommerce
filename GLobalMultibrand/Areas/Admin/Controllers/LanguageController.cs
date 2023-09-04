using GMB.Business.Helpers;
using GMB.Business.ViewModels.Languages;
using GMB.Core.Entities;
using GMB.DAL.DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Watch.BLL.Extensions;

namespace GLobalMultibrand.Areas.Admin.Controllers
{
    public class LanguageController : BaseController
    {
        private readonly GMBDbContext _dbContext;

        public LanguageController(GMBDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IActionResult> Index()
        {
            var languages = await _dbContext.Languages.Where(l => !l.IsDeleted).ToListAsync();
            return View(languages);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(LanguageCreateViewModel model)
        {
            if(!ModelState.IsValid)
            {
                return View();
            }

            if (!model.Image.IsImage())
            {
                ModelState.AddModelError("Image", "An image must be selected..!");
                return View(model);
            }

            if (!model.Image.IsAllowedSize(10))
            {
                ModelState.AddModelError("Image", "Image size can be maximum 50mb..!");
                return View(model);
            }

            var unicalFileName = await model.Image.GenerateFile(Constants.LanguageFlagPath);

            var language = new Language
            {
                ImageUrl = unicalFileName,
                Name = model.Name,
                IsoCode = model.IsoCode

            };
            await _dbContext.Languages.AddAsync(language);
            await _dbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
