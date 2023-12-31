﻿using GMB.Business.ViewModels.Colors;
using GMB.Core.Entities;
using GMB.DAL.DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GLobalMultibrand.Areas.Admin.Controllers
{
    public class ColorController : BaseController
    {
        private readonly GMBDbContext _dbContext;
        public ColorController(GMBDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<IActionResult> Index()
        {
            var colors = await _dbContext.Colors.Where(c => !c.IsDeleted).OrderByDescending(bc => bc.Id).ToListAsync();
            return View(colors);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ColorCreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            var existColors = await _dbContext.Colors.Where(c => !c.IsDeleted).ToListAsync();

            if (existColors.Any(c => c.Name.ToLower().Equals(model.Name.ToLower())))
            {
                ModelState.AddModelError("Name", "There is a color with this name..! ");
                return View();
            }
            var newColor = new Color
            {
                Name = model.Name
            };

            await _dbContext.Colors.AddAsync(newColor);
            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));

        }

        public async Task<IActionResult> Update(int? id)
        {
            if (id is null) return NotFound();
            var color = await _dbContext.Colors.FindAsync(id);
            if (color.Id != id) return BadRequest();

            var existColor = new ColorUpdateViewModel
            {
                Name = color.Name
            };
            return View(existColor);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int? id, ColorUpdateViewModel model)
        {

            if (id is null) return NotFound();

            if (!ModelState.IsValid)
            {
                return View();
            }

            var color = await _dbContext.Colors.FindAsync(id);

            if (color is null) return NotFound();
            var isExistName = await _dbContext.Colors.AnyAsync(c => c.Name.ToLower() == model.Name.ToLower() && c.Id != id);

            if (isExistName)
            {
                ModelState.AddModelError("Name", "There is a color with this name..!");
                return View(model);
            }
            color.Name = model.Name;

            await _dbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id is null) return NotFound();

            var color = await _dbContext.Colors.FindAsync(id);

            if (color is null) return NotFound();

            _dbContext.Colors.Remove(color);

            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));

        }
    }
}
