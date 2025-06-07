using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using Pab.WebAdmin.Models;
using System.Net.Http.Headers;

namespace Pab.WebAdmin.Pages.Account
{
    public class LoginModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        public LoginModel(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }

        [BindProperty]
        public LoginViewModel Input { get; set; } = new LoginViewModel();

        public IActionResult OnGet()
        {
            // Jeœli ju¿ jest token w sesji, przekieruj na Index
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("JWToken")))
            {
                return RedirectToPage("/Index");
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Przygotuj HttpClient
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["ApiBaseUrl"]!);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var loginReq = new LoginRequest
            {
                Email = Input.Email,
                Password = Input.Password
            };
            var jsonContent = JsonConvert.SerializeObject(loginReq);
            var httpContent = new StringContent(jsonContent, System.Text.Encoding.UTF8, "application/json");

            // Wyœlij POST /api/Auth/login
            var response = await client.PostAsync("api/Auth/login", httpContent);

            if (!response.IsSuccessStatusCode)
            {
                Input.ErrorMessage = "Nieprawid³owy e-mail lub has³o.";
                return Page();
            }

            // Jeœli OK – odczytaj token
            var respJson = await response.Content.ReadAsStringAsync();
            var loginResp = JsonConvert.DeserializeObject<TokenResponse>(respJson)!;

            // Zapisz token w sesji
            HttpContext.Session.SetString("JWToken", loginResp.Token);

            // Przekieruj na Index
            return RedirectToPage("/Index");
        }
    }
}
