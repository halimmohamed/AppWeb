using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAppSafeJourney.Data;
using WebAppSafeJourney.Models.Enums;

namespace WebAppSafeJourney.ViewComponents
{
    public class FeaturedGuidesViewComponent : ViewComponent
    {
        private readonly AppDbContext _context;

        public FeaturedGuidesViewComponent(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync(int count = 6)
        {
            var guides = await _context.Guides.AsNoTracking()
                .Include(g => g.User)
                .Include(g => g.CoveredRegions)
                .Where(g => g.Status == GuideStatus.Approved)
                .OrderByDescending(g => g.ApprovedAt)
                .Take(count)
                .ToListAsync();

            return View("Default", guides);
        }
    }
}