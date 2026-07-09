using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using WebAppSafeJourney.Data;
using WebAppSafeJourney.Models;
using WebAppSafeJourney.ViewModels;

namespace WebAppSafeJourney.Controllers
{
    public class AccountController : Controller
    {
        private readonly AppDbContext _context;

        public AccountController(AppDbContext context)
        {
            _context = context;
        }

        // ==================== REGISTER ====================
        [HttpGet]
        public IActionResult Register()
        {
            // لو المستخدم مسجل دخول بالفعل، ينقله للصفحة الرئيسية
            if (User.Identity != null && User.Identity.IsAuthenticated)
                return RedirectToAction("Index", "Home");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterVM model)
        {
            if (!ModelState.IsValid) return View(model);

            // التحقق من أن الإيميل مش مكرر
            var userExists = await _context.Users.AnyAsync(u => u.Email == model.Email);
            if (userExists)
            {
                ModelState.AddModelError("Email", "This email address is already in use");
                return View(model);
            }

            // تشفير كلمة المرور وحفظ المستخدم الجديد
            // في الـ Register POST Action:
            var newUser = new User
            {
                FullName = model.FullName,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(model.Password),

                // --- التعديل الأمني هنا ---
                // نتأكد إنه لا يمكن لأي مستخدم تسجيل نفسه كـ Admin مهما حاول
                Role = (model.Role == WebAppSafeJourney.Models.Enums.UserRole.Admin)
                        ? WebAppSafeJourney.Models.Enums.UserRole.Tourist
                        : model.Role,
                // -------------------------

                CreatedAt = DateTime.Now
            };

            await _context.Users.AddAsync(newUser);
            await _context.SaveChangesAsync();

            // === التعديل الجديد: إنشاء ملف شخصي للمرشد تلقائياً إذا اختار دور المرشد ===
            if (newUser.Role == WebAppSafeJourney.Models.Enums.UserRole.Guide)
            {
                var guideProfile = new WebAppSafeJourney.Models.Guide
                {
                    UserId = newUser.Id,
                    Status = WebAppSafeJourney.Models.Enums.GuideStatus.Approved,
                    LicenseNumber = "Pending Update",
                    NationalId = "Pending Update"
                };
                _context.Guides.Add(guideProfile);
                await _context.SaveChangesAsync();
            }
            // =======================================================================

            return RedirectToAction("Login", "Account");
        }

        // ==================== LOGIN ====================
        [HttpGet]
        public IActionResult Login()
        {
            if (User.Identity != null && User.Identity.IsAuthenticated)
                return RedirectToAction("Index", "Home");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginVM model)
        {
            if (!ModelState.IsValid) return View(model);

            // البحث عن المستخدم بالإيميل
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == model.Email);
            if (user == null || !BCrypt.Net.BCrypt.Verify(model.Password, user.PasswordHash))
            {
                ModelState.AddModelError(string.Empty, "Invalid login attempt. Please check your credentials.");
                return View(model);
            }

            // إعداد الهوية (Claims) لحفظ جلسة المستخدم في الـ Cookie
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.FullName ?? string.Empty),
                new Claim(ClaimTypes.Email, user.Email ?? string.Empty),
                new Claim(ClaimTypes.Role, user.Role.ToString())
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties
            {
                IsPersistent = model.RememberMe,
                ExpiresUtc = DateTimeOffset.UtcNow.AddDays(7)
            };

            // تسجيل الدخول الفعلي
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);

            return RedirectToAction("Index", "Home");
        }

        // ==================== LOGOUT ====================
        [HttpPost]
        [ValidateAntiForgeryToken] // دي أهم حاجة عشان نمنع أي تلاعب من مواقع تانية
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }

        // صفحة حظر الوصول (عندما يحاول مستخدم دخول صفحة أدمن مثلاً)
        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}