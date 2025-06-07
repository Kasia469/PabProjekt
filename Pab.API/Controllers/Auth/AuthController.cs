using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Pab.Domain.Entities;      // zakładam, że ApplicationUser leży w Pab.Domain.Entities
using Pab.Domain.Identity;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Pab.API.Controllers.Auth
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;

        public AuthController(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IConfiguration configuration)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
        }

        // DTO do rejestracji
        public class RegisterRequest
        {
            public string UserName { get; set; } = null!;
            public string Email { get; set; } = null!;
            public string Password { get; set; } = null!;
        }

        // DTO do logowania
        public class LoginRequest
        {
            public string Email { get; set; } = null!;
            public string Password { get; set; } = null!;
        }

        /// <summary>
        /// Rejestracja nowego użytkownika:
        /// - Tworzy rolę "User", jeśli nie istnieje
        /// - Tworzy konto ApplicationUser
        /// - Nadaje użytkownikowi rolę "User"
        /// </summary>
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            // 1) Sprawdź, czy użytkownik o podanym UserName lub Email już istnieje
            var userByName = await _userManager.FindByNameAsync(request.UserName);
            if (userByName != null)
                return BadRequest(new { message = "Nazwa użytkownika już zajęta." });

            var userByEmail = await _userManager.FindByEmailAsync(request.Email);
            if (userByEmail != null)
                return BadRequest(new { message = "Ten e-mail jest już zarejestrowany." });

            // 2) Utwórz obiekt ApplicationUserb
            var user = new ApplicationUser
            {
                UserName = request.UserName,
                Email = request.Email,
                EmailConfirmed = true // można zostawić true, żeby nie wymagać potwierdzenia przez link
            };

            // 3) Zapisz użytkownika w bazie (domyślnie używa .AddPasswordHasherAsync)
            var createResult = await _userManager.CreateAsync(user, request.Password);
            if (!createResult.Succeeded)
            {
                // jeśli nie powiodło się utworzenie, zwróć wszystkie błędy
                var errors = createResult.Errors.Select(e => e.Description).ToList();
                return BadRequest(new { message = "Rejestracja nie powiodła się.", errors });
            }

            // 4) Upewnij się, że rola "User" istnieje
            const string defaultRole = "User";
            if (!await _roleManager.RoleExistsAsync(defaultRole))
            {
                var roleResult = await _roleManager.CreateAsync(new IdentityRole(defaultRole));
                if (!roleResult.Succeeded)
                {
                    // jeśli nie udało się stworzyć roli, zwróć błąd
                    var roleErrors = roleResult.Errors.Select(e => e.Description).ToList();
                    return StatusCode(500, new { message = "Nie udało się utworzyć roli.", roleErrors });
                }
            }

            // 5) Przypisz utworzonemu użytkownikowi rolę "User"
            var addToRoleResult = await _userManager.AddToRoleAsync(user, defaultRole);
            if (!addToRoleResult.Succeeded)
            {
                // jeżeli coś poszło nie tak, usuń utworzone już konto i zwróć błąd
                await _userManager.DeleteAsync(user);
                var atribErrors = addToRoleResult.Errors.Select(e => e.Description).ToList();
                return StatusCode(500, new { message = "Nie udało się przypisać roli użytkownikowi.", atribErrors });
            }

            // 6) Wszystko OK – zwróć komunikat
            return Ok(new { message = "Rejestracja zakończona sukcesem. Możesz się teraz zalogować." });
        }

        /// <summary>
        /// Logowanie – w zamian zwraca token JWT:
        /// - Sprawdza istniejącego użytkownika wg e-maila i weryfikuje hasło
        /// - Pobiera role użytkownika
        /// - Generuje token JWT zawierający Claimy: Id, Email, Role
        /// </summary>
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            // 1) Znajdź użytkownika po adresie e-mail
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
                return Unauthorized(new { message = "Nieprawidłowy e-mail lub hasło." });

            // 2) Sprawdź, czy hasło jest poprawne
            var passwordOk = await _userManager.CheckPasswordAsync(user, request.Password);
            if (!passwordOk)
                return Unauthorized(new { message = "Nieprawidłowy e-mail lub hasło." });

            // 3) Pobierz role (może być wiele ról, np. Admin, User)
            var roles = await _userManager.GetRolesAsync(user);

            // 4) Przygotuj listę Claim-ów do tokenu
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id),          // identyfikator użytkownika (z domyślnego IdentityUser.Id)
                new Claim(JwtRegisteredClaimNames.Email, user.Email),     // e-mail
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()) // unikalny identyfikator tokenu
            };

            // Dodaj do claimów wszystkie role
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            // 5) Klucz do podpisywania JWT (pobrany z appsettings.json)
            var jwtKey = _configuration["Jwt:Key"];
            if (string.IsNullOrEmpty(jwtKey))
                return StatusCode(500, new { message = "Brak klucza JWT w konfiguracji." });

            var keyBytes = Encoding.UTF8.GetBytes(jwtKey);
            var securityKey = new SymmetricSecurityKey(keyBytes);
            var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            // 6) Tworzenie tokenu
            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(2),
                signingCredentials: signingCredentials
            );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            // 7) Zwróć token w odpowiedzi
            return Ok(new
            {
                token = tokenString,
                expiration = token.ValidTo
            });
        }
    }
}
