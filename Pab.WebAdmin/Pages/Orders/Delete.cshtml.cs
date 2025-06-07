// Pab.WebAdmin/Pages/Orders/Delete.cshtml.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using Pab.WebAdmin.Models;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Pab.WebAdmin.Pages.Orders
{
    public class DeleteModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        public DeleteModel(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }

        [BindProperty]
        public OrderDto Order { get; set; } = default!;

        // Wyœwietl szczegó³y przed usuniêciem
        public async Task<IActionResult> OnGetAsync(int id)
        {
            var token = HttpContext.Session.GetString("JWToken");
            if (string.IsNullOrEmpty(token))
                return RedirectToPage("/Account/Login");

            var client = _httpClientFactory.CreateClient("PabApiClient");
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

            var response = await client.GetAsync($"api/Orders/{id}");
            if (!response.IsSuccessStatusCode)
                return RedirectToPage("./Index");

            var json = await response.Content.ReadAsStringAsync();
            Order = JsonConvert.DeserializeObject<OrderDto>(json)!;
            return Page();
        }

        // Po potwierdzeniu – usuñ
        public async Task<IActionResult> OnPostAsync()
        {
            var token = HttpContext.Session.GetString("JWToken");
            if (string.IsNullOrEmpty(token))
                return RedirectToPage("/Account/Login");

            var client = _httpClientFactory.CreateClient("PabApiClient");
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

            var response = await client.DeleteAsync($"api/Orders/{Order.Id}");
            // nawet jeœli b³¹d, po prostu wróæmy do Index
            return RedirectToPage("./Index");
        }
    }
}
