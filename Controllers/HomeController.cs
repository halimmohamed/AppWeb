using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAppSafeJourney.Data;
using WebAppSafeJourney.Models;
using WebAppSafeJourney.Models.Enums;
using WebAppSafeJourney.ViewModels;
using Microsoft.Extensions.Caching.Memory;


namespace WebAppSafeJourney.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IMemoryCache _cache; // السطر الجديد

        public HomeController(AppDbContext context, IMemoryCache cache)
        {
            _context = context;
            _cache = cache; // السطر الجديد
        }
        public IActionResult Privacy() => View();

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() =>
            View(new ErrorViewModel { RequestId = System.Diagnostics.Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        // عرض الصفحة الرئيسية وبها المزارات السياحية للجمهور
        public async Task<IActionResult> Index(string? search, EgyptRegion? region)
        {
            // سحب البيانات من الكاش أو الداتابيز لو الكاش فاضي
            var destinations = await _cache.GetOrCreateAsync("active_destinations", async entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5); // كاش لمدة 5 دقايق
                return await _context.Destinations.AsNoTracking().Where(d => d.IsActive).ToListAsync();
            });

            // فلترة البيانات اللي في الكاش (سريعة جداً لأنها في الميموري)
            var query = destinations!.AsEnumerable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(d => d.Name!.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                                         d.Description!.Contains(search, StringComparison.OrdinalIgnoreCase));
            }

            if (region.HasValue)
            {
                query = query.Where(d => d.Region == region.Value);
            }

            ViewBag.SearchTerm = search;
            ViewBag.SelectedRegion = region;

            return View(query.OrderByDescending(d => d.CreatedAt).ToList());
        }

        // دالة Details الموحدة والمعدلة
        public async Task<IActionResult> Details(int id)
        {
            var destination = await _context.Destinations
                .Where(d => d.Id == id && d.IsActive)
                .Select(d => new DestinationDetailsVM
                {
                    Id = d.Id,
                    Name = d.Name,
                    Description = d.Description,
                    Region = d.Region,
                    CoverImageUrl = d.CoverImageUrl,
                    Reviews = d.Reviews.Select(r => new ReviewDisplayVM
                    {
                        TouristName = r.Tourist != null && r.Tourist.FullName != null ? r.Tourist.FullName : "Unknown",
                        Rating = r.Rating,
                        Comment = r.Comment ?? string.Empty,
                        CreatedAt = r.CreatedAt
                    }).ToList()
                })
                .FirstOrDefaultAsync();

            if (destination == null)
            {
                return NotFound();
            }

            return View(destination);
        }
    }
}