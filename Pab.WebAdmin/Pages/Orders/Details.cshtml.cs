// Pab.WebAdmin/Pages/Orders/Details.cshtml.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using Pab.WebAdmin.Models; // Zakładam, że masz klasę OrderDto w Projects/Models
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Pab.WebAdmin.Pages.Orders
{
    public class DetailsModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        public DetailsModel(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }

        [BindProperty]
        public OrderDto Order { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int id)
        {
            // 1) Pobierz JWT z sesji
            var token = HttpContext.Session.GetString("JWToken");
            if (string.IsNullOrEmpty(token))
            {
                return RedirectToPage("/Account/Login");
            }

            // 2) Przygotuj HttpClient
            var client = _httpClientFactory.CreateClient("PabApiClient");
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

            // 3) Wyślij GET na endpoint api/Orders/{id}
            var response = await client.GetAsync($"api/Orders/{id}");
            if (!response.IsSuccessStatusCode)
            {
                // np. 404 albo 401 → wróć do listy
                return RedirectToPage("./Index");
            }

            var json = await response.Content.ReadAsStringAsync();
            Order = JsonConvert.DeserializeObject<OrderDto>(json)!;

            return Page();
        }
    }
}
