using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using WebAppSafeJourney.Data;
using WebAppSafeJourney.Models.Enums;

namespace WebAppSafeJourney.ViewComponents
{
    public class GuideRequestBadgeViewComponent : ViewComponent
    {
        private readonly AppDbContext _context;

        public GuideRequestBadgeViewComponent(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var userIdValue = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdValue) || !int.TryParse(userIdValue, out var userId))
                return Content(string.Empty);

            var myProfile = await _context.Guides.AsNoTracking()
                .FirstOrDefaultAsync(g => g.UserId == userId);

            if (myProfile == null) return Content(string.Empty);

            var pendingCount = await _context.GuideBookings.AsNoTracking()
                .CountAsync(b => b.GuideId == myProfile.Id && b.Status == GuideBookingStatus.Pending);

            return View("Default", pendingCount);
        }
    }
}