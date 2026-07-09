using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAppSafeJourney.Data;
using WebAppSafeJourney.Models.Enums;

namespace WebAppSafeJourney.Controllers
{
    // بنحمي الكنترولر ده عشان محدش يدخله غير الأدمن بس
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly AppDbContext _context;

        public AdminController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            // تجميع الإحصائيات
            ViewBag.TotalTourists = await _context.Users.CountAsync(u => u.Role == UserRole.Tourist);
            ViewBag.TotalGuides = await _context.Users.CountAsync(u => u.Role == UserRole.Guide);
            ViewBag.TotalDestinations = await _context.Destinations.CountAsync();

            // عدد الحجوزات اللي لسه Pending
            ViewBag.PendingBookings = await _context.TripBookings
                .CountAsync(b => b.Status == TripBookingStatus.Pending);

            // أحدث 5 حجوزات لعرضهم في الداشبورد
            var recentBookings = await _context.TripBookings
                .Include(b => b.Tourist)
                .Include(b => b.Destination)
                .OrderByDescending(b => b.CreatedAt)
                .Take(5)
                .ToListAsync();

            return View(recentBookings);
        }
    }
}