using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAppSafeJourney.Data;
using WebAppSafeJourney.Models;
using WebAppSafeJourney.ViewModels;

namespace WebAppSafeJourney.Controllers
{
    [Authorize(Roles = "Admin")]
    public class DestinationsController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public DestinationsController(AppDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: Destinations
        public async Task<IActionResult> Index()
        {
            var allDestinations = await _context.Destinations.AsNoTracking().ToListAsync();
            return View(allDestinations);
        }

        // GET: Destinations/Create
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Destinations/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DestinationVM model)
        {
            if (ModelState.IsValid)
            {
                string imageUrl = model.CoverImageUrl ?? string.Empty;

                // لو الأدمن رفع صورة من جهازه، يحفظها في السيرفر
                if (model.ImageFile != null && model.ImageFile.Length > 0)
                {
                    string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images", "destinations");
                    if (!Directory.Exists(uploadsFolder))
                        Directory.CreateDirectory(uploadsFolder);

                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(model.ImageFile.FileName);
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await model.ImageFile.CopyToAsync(fileStream);
                    }

                    imageUrl = "/images/destinations/" + uniqueFileName;
                }

                var destination = new Destination
                {
                    Name = model.Name,
                    Description = model.Description,
                    CoverImageUrl = imageUrl,
                    Region = model.Region,
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                };

                _context.Add(destination);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // GET: Destinations/Edit/5
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var destination = await _context.Destinations.FindAsync(id);
            if (destination == null) return NotFound();

            var model = new DestinationVM
            {
                Id = destination.Id,
                Name = destination.Name ?? string.Empty,
                Description = destination.Description ?? string.Empty,
                CoverImageUrl = destination.CoverImageUrl,
                Region = destination.Region
            };

            return View(model);
        }

        // POST: Destinations/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, DestinationVM model)
        {
            if (id != model.Id) return NotFound();

            if (ModelState.IsValid)
            {
                var destination = await _context.Destinations.FindAsync(id);
                if (destination == null) return NotFound();

                destination.Name = model.Name;
                destination.Description = model.Description;
                destination.Region = model.Region;

                // لو الأدمن رفع صورة جديدة أثناء التعديل
                if (model.ImageFile != null && model.ImageFile.Length > 0)
                {
                    string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images", "destinations");
                    if (!Directory.Exists(uploadsFolder))
                        Directory.CreateDirectory(uploadsFolder);

                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(model.ImageFile.FileName);
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await model.ImageFile.CopyToAsync(fileStream);
                    }

                    destination.CoverImageUrl = "/images/destinations/" + uniqueFileName;
                }
                else if (!string.IsNullOrEmpty(model.CoverImageUrl))
                {
                    destination.CoverImageUrl = model.CoverImageUrl;
                }

                try
                {
                    _context.Update(destination);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DestinationExists(destination.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // POST: Destinations/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var destination = await _context.Destinations.FindAsync(id);
            if (destination == null) return NotFound();

            _context.Destinations.Remove(destination);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DestinationExists(int id)
        {
            return _context.Destinations.Any(e => e.Id == id);
        }
    }
}