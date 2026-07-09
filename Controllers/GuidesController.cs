using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAppSafeJourney.Data;
using WebAppSafeJourney.Models.Enums;

namespace WebAppSafeJourney.Controllers
{
    public class GuidesController : Controller
    {
        private readonly AppDbContext _context;

        public GuidesController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(EgyptRegion? region)
        {
            var query = _context.Guides.AsNoTracking()
                .Include(g => g.User)
                .Where(g => g.Status == GuideStatus.Approved);

            if (region.HasValue)
            {
                query = query.Where(g => g.CoveredRegions!.Any(r => r.Region == region.Value));
                ViewBag.SelectedRegion = region.Value.ToString();
            }
            else
            {
                ViewBag.SelectedRegion = "All Regions";
            }

            var guides = await query.ToListAsync();
            return View(guides);
        }
    }
}