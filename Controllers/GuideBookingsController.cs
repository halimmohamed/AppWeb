using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using WebAppSafeJourney.Data;
using WebAppSafeJourney.Models;
using WebAppSafeJourney.Models.Enums;
using WebAppSafeJourney.ViewModels;

namespace WebAppSafeJourney.Controllers
{
    [Authorize]
    public class GuideBookingsController : Controller
    {
        private readonly AppDbContext _context;

        public GuideBookingsController(AppDbContext context)
        {
            _context = context;
        }

        private int CurrentUserId => int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

        [HttpGet]
        [Authorize(Roles = "Tourist")]
        public async Task<IActionResult> Create(int guideId)
        {
            var guide = await _context.Guides
                .Include(g => g.User)
                .FirstOrDefaultAsync(g => g.Id == guideId && g.Status == GuideStatus.Approved);

            if (guide == null)
            {
                TempData["Error"] = "This guide is currently unavailable for booking.";
                return RedirectToAction("Index", "Guides");
            }

            ViewBag.GuideName = guide.User?.FullName;
            ViewBag.Destinations = new SelectList(
                await _context.Destinations.Where(d => d.IsActive).ToListAsync(), "Id", "Name");

            return View(new GuideBookingCreateVM { GuideId = guide.Id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Tourist")]
        public async Task<IActionResult> Create(GuideBookingCreateVM model)
        {
            var guide = await _context.Guides.Include(g => g.User)
                .FirstOrDefaultAsync(g => g.Id == model.GuideId && g.Status == GuideStatus.Approved);
            var destination = await _context.Destinations
                .FirstOrDefaultAsync(d => d.Id == model.DestinationId && d.IsActive);

            if (guide == null)
                ModelState.AddModelError("", "This guide is unavailable for booking.");
            if (destination == null)
                ModelState.AddModelError(nameof(model.DestinationId), "The selected destination is unavailable.");
            if (model.TourDate.Date <= DateTime.Today)
                ModelState.AddModelError(nameof(model.TourDate), "The tour date must be in the future.");

            if (!ModelState.IsValid)
            {
                ViewBag.GuideName = guide?.User?.FullName;
                ViewBag.Destinations = new SelectList(
                    await _context.Destinations.Where(d => d.IsActive).ToListAsync(), "Id", "Name", model.DestinationId);
                return View(model);
            }

            var booking = new GuideBooking
            {
                TouristId = CurrentUserId,
                GuideId = model.GuideId,
                DestinationId = model.DestinationId,
                TourDate = model.TourDate,
                DurationDays = model.DurationDays,
                NumberOfPeople = model.NumberOfPeople,
                SpecialNotes = model.SpecialNotes,
                Status = GuideBookingStatus.Pending,
                CreatedAt = DateTime.Now
            };

            _context.GuideBookings.Add(booking);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Booking request sent to the guide successfully.";
            return RedirectToAction(nameof(MyBookings));
        }

        [Authorize(Roles = "Tourist")]
        public async Task<IActionResult> MyBookings()
        {
            var bookings = await _context.GuideBookings.AsNoTracking()
                .Include(b => b.Guide!).ThenInclude(g => g!.User)
                .Include(b => b.Destination)
                .Where(b => b.TouristId == CurrentUserId)
                .OrderByDescending(b => b.CreatedAt)
                .ToListAsync();

            return View(bookings);
        }

        [Authorize(Roles = "Guide")]
        public async Task<IActionResult> IncomingRequests()
        {
            var myProfile = await _context.Guides.FirstOrDefaultAsync(g => g.UserId == CurrentUserId);
            if (myProfile == null) return View(new List<GuideBooking>());

            var requests = await _context.GuideBookings.AsNoTracking()
                .Include(b => b.Tourist)
                .Include(b => b.Destination)
                .Where(b => b.GuideId == myProfile.Id)
                .OrderByDescending(b => b.CreatedAt)
                .ToListAsync();

            return View(requests);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Guide")]
        public async Task<IActionResult> Confirm(int id, string? message)
        {
            var myProfile = await _context.Guides.FirstOrDefaultAsync(g => g.UserId == CurrentUserId);
            var booking = await _context.GuideBookings.FirstOrDefaultAsync(b => b.Id == id);

            if (booking == null || myProfile == null || booking.GuideId != myProfile.Id)
                return Forbid();

            if (booking.Status != GuideBookingStatus.Pending)
            {
                TempData["Error"] = "Cannot modify a request that has already been processed.";
                return RedirectToAction(nameof(IncomingRequests));
            }

            booking.Status = GuideBookingStatus.Confirmed;
            booking.GuideResponseMessage = message;
            await _context.SaveChangesAsync();

            TempData["Success"] = "Request accepted successfully.";
            return RedirectToAction(nameof(IncomingRequests));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Guide")]
        public async Task<IActionResult> Reject(int id, string? reason)
        {
            var myProfile = await _context.Guides.FirstOrDefaultAsync(g => g.UserId == CurrentUserId);
            var booking = await _context.GuideBookings.FirstOrDefaultAsync(b => b.Id == id);

            if (booking == null || myProfile == null || booking.GuideId != myProfile.Id)
                return Forbid();

            if (booking.Status != GuideBookingStatus.Pending)
            {
                TempData["Error"] = "Cannot modify a request that has already been processed.";
                return RedirectToAction(nameof(IncomingRequests));
            }

            booking.Status = GuideBookingStatus.Rejected;
            booking.RejectionReason = reason;
            await _context.SaveChangesAsync();

            TempData["Success"] = "Request rejected.";
            return RedirectToAction(nameof(IncomingRequests));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Guide")]
        public async Task<IActionResult> MarkCompleted(int id)
        {
            var myProfile = await _context.Guides.FirstOrDefaultAsync(g => g.UserId == CurrentUserId);
            var booking = await _context.GuideBookings.FirstOrDefaultAsync(b => b.Id == id);

            if (booking == null || myProfile == null || booking.GuideId != myProfile.Id)
                return Forbid();

            if (booking.Status != GuideBookingStatus.Confirmed)
            {
                TempData["Error"] = "The request must be confirmed before marking it as completed.";
                return RedirectToAction(nameof(IncomingRequests));
            }

            booking.Status = GuideBookingStatus.Completed;
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(IncomingRequests));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Tourist")]
        public async Task<IActionResult> Cancel(int id)
        {
            var booking = await _context.GuideBookings.FirstOrDefaultAsync(b => b.Id == id);

            if (booking == null || booking.TouristId != CurrentUserId)
                return Forbid();

            if (booking.Status != GuideBookingStatus.Pending)
            {
                TempData["Error"] = "Cannot cancel a request that has already been processed.";
                return RedirectToAction(nameof(MyBookings));
            }

            booking.Status = GuideBookingStatus.Cancelled;
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(MyBookings));
        }
    }
}