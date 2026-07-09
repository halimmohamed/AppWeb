using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebAppSafeJourney.Data;
using WebAppSafeJourney.Models;
using WebAppSafeJourney.ViewModels;

[Authorize]
public class ReviewsController : Controller
{
    private readonly AppDbContext _context;
    public ReviewsController(AppDbContext context) => _context = context;

    [HttpPost]
    [ValidateAntiForgeryToken] // CSRF Protection
    public async Task<IActionResult> Create(ReviewCreateVM model)
    {
        if (!ModelState.IsValid)
        {
            // If validation fails, return to the destination page with errors
            // You might need to use TempData to pass errors back if you are redirecting
            TempData["ErrorMessage"] = "Please fill all required fields correctly.";
            return RedirectToAction("Details", "Home", new { id = model.DestinationId });
        }

        // Assuming you get the tourist ID from the logged-in user claims
        var touristIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
        int touristId = int.Parse(touristIdString ?? "0");

        var review = new Review
        {
            DestinationId = model.DestinationId,
            Rating = model.Rating,
            Comment = model.Comment,
            TouristId = touristId, // Now it's an integer
            CreatedAt = DateTime.UtcNow
        };

        _context.Reviews.Add(review);
        await _context.SaveChangesAsync();

        TempData["SuccessMessage"] = "Review added successfully!";
        return RedirectToAction("Details", "Home", new { id = model.DestinationId });
    }
}