using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using WebAppSafeJourney.Data;
using WebAppSafeJourney.Models;
using WebAppSafeJourney.Models.Enums;
using WebAppSafeJourney.ViewModels;
namespace WebAppSafeJourney.Controllers
{
    [Authorize]
    public class BookingsController : Controller
    {
        private readonly AppDbContext _context;

        public BookingsController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            if (User.IsInRole("Admin"))
            {
                var allBookings = await _context.TripBookings
                    .Include(b => b.Tourist)
                    .Include(b => b.Destination)
                    .OrderByDescending(b => b.CreatedAt)
                    .ToListAsync();
                return View(allBookings);
            }
            else
            {
                var touristBookings = await _context.TripBookings
                    .Include(b => b.Destination)
                    .Where(b => b.TouristId == userId)
                    .OrderByDescending(b => b.CreatedAt)
                    .ToListAsync();
                return View(touristBookings);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken] // CSRF Protection
        public async Task<IActionResult> CreateRequest(TripBookingCreateVM model)
        {
            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Invalid booking details. Please try again.";
                return RedirectToAction("Details", "Home", new { id = model.DestinationId });
            }

            var touristIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            int touristId = int.Parse(touristIdString ?? "0");

            var tripBooking = new TripBooking
            {
                DestinationId = model.DestinationId,
                TripType = model.TripType,
                StartDate = model.StartDate,
                EndDate = model.EndDate,
                NumberOfPeople = model.NumberOfPeople,
                SpecialNotes = model.SpecialNotes,
                TouristId = touristId, // Now it's an integer
                CreatedAt = DateTime.UtcNow,
                Status = TripBookingStatus.Pending
            };

            _context.TripBookings.Add(tripBooking);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Booking request submitted successfully!";
            return RedirectToAction("Details", "Home", new { id = model.DestinationId });
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateStatus(int bookingId, TripBookingStatus status)
        {
            var booking = await _context.TripBookings.FindAsync(bookingId);
            if (booking == null) return NotFound();

            booking.Status = status;
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}