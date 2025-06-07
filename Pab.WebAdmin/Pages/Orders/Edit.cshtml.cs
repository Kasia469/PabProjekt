// Pab.WebAdmin/Pages/Orders/Edit.cshtml.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using Pab.WebAdmin.Models;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Pab.WebAdmin.Pages.Orders
{
    public class EditModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        public EditModel(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }

        [BindProperty]
        public OrderDto Order { get; set; } = default!;

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

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            var token = HttpContext.Session.GetString("JWToken");
            if (string.IsNullOrEmpty(token))
                return RedirectToPage("/Account/Login");

            var client = _httpClientFactory.CreateClient("PabApiClient");
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

            // Serializuj obiekt i wyœlij PUT na api/Orders/{id}
            var jsonContent = JsonConvert.SerializeObject(Order);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            var response = await client.PutAsync($"api/Orders/{Order.Id}", content);

            if (!response.IsSuccessStatusCode)
            {
                // np. b³¹d walidacji
                ModelState.AddModelError(string.Empty, "Nie uda³o siê zaktualizowaæ zamówienia");
                return Page();
            }

            return RedirectToPage("./Index");
        }
    }
}
