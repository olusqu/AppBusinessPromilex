using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebBusinessPromilexApp.Models; 

namespace WebBusinessPromilexApp.Services
{
    public class AuthenticationService
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthenticationService(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<bool> LoginAsync(string username, string password)
        {
         
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                return false;
            }

            // Szukam użytkownika w bazie po podanym loginie
            var user = await _context.Employees.FirstOrDefaultAsync(e => e.Username == username);

            // Jeśli znaleziono użytkownika
            if (user != null)
            {
                var hasher = new PasswordHasher<Employee>();
                if (user.PasswordHash != null)
                {
                    var result = hasher.VerifyHashedPassword(user, user.PasswordHash, password);

                    // Jeśli hasło jest poprawne, logujemy go
                    if (result == PasswordVerificationResult.Success)
                    {
                        var session = _httpContextAccessor.HttpContext.Session;

                        session.SetString("IsAdminLoggedIn", "true");
                        session.SetInt32("EmployeeId", user.Id);

                        session.SetString("EmployeeName", user.Username ?? "Pracownik");

                        return true;
                    }
                }
            }

            // Jeśli użytkownik nie istnieje lub hasło jest błędne
            return false;
        }

        public void Logout()
        {
            var session = _httpContextAccessor.HttpContext.Session;
            session.Clear();
        }
    }
}