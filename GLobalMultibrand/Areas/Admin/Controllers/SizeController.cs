using GMB.Business.ViewModels;
using GMB.Business.ViewModels.Sizes;
using GMB.Core.Entities;
using GMB.DAL.DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GLobalMultibrand.Areas.Admin.Controllers
{
    public class SizeController : BaseController
    {
        private readonly GMBDbContext _dbContext;

        public SizeController(GMBDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IActionResult> Index()
        {
            var sizes = await _dbContext.Sizes.Where(c => !c.IsDeleted).OrderByDescending(bc => bc.Id).ToListAsync();
            return View(sizes);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SizeCreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            var existSizes = await _dbContext.Sizes.Where(c => !c.IsDeleted).ToListAsync();

            if (existSizes.Any(c => c.Name.ToLower().Equals(model.Name.ToLower())))
            {
                ModelState.AddModelError("Name", "There is a size with this name..! ");
                return View();
            }
            var newSize = new Size
            {
                Name = model.Name
            };

            await _dbContext.Sizes.AddAsync(newSize);
            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));

        }

        public async Task<IActionResult> Update(int? id)
        {
            if (id is null) return NotFound();
            var size = await _dbContext.Sizes.FindAsync(id);
            if (size.Id != id) return BadRequest();

            var existSize = new SizeUpdateViewModel
            {
                Name = size.Name
            };
            return View(existSize);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int? id, SizeUpdateViewModel model)
        {

            if (id is null) return NotFound();

            if (!ModelState.IsValid)
            {
                return View();
            }

            var size = await _dbContext.Sizes.FindAsync(id);

            if (size is null) return NotFound();
            var isExistName = await _dbContext.Sizes.AnyAsync(c => c.Name.ToLower() == model.Name.ToLower() && c.Id != id);

            if (isExistName)
            {
                ModelState.AddModelError("Name", "There is a size with this name..!");
                return View(model);
            }
            size.Name = model.Name;

            await _dbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

           public async Task<IActionResult> Delete(int? id)
        {
            if (id is null) return NotFound();

            var size = await _dbContext.Sizes.FindAsync(id);

            if (size is null) return NotFound();

            _dbContext.Sizes.Remove(size);

            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));

        }


    }
}
