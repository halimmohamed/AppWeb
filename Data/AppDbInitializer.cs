using WebAppSafeJourney.Models;
using WebAppSafeJourney.Models.Enums;
using BCrypt.Net;
namespace WebAppSafeJourney.Data
{
    public class AppDbInitializer
    {
        public static void Seed(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<AppDbContext>()!;
                context.Database.EnsureCreated();

                // إنشاء حساب Admin إذا لم يكن موجوداً
                if (!context.Users.Any(u => u.Role == UserRole.Admin))
                {
                    context.Users.Add(new User()
                    {
                        FullName = "System Administrator",
                        Email = "admin@safejourney.com",
                        PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin123!"),
                        Role = UserRole.Admin,
                        CreatedAt = DateTime.Now
                    });
                    context.SaveChanges();
                }
            }
        }
    }
}