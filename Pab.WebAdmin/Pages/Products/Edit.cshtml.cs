using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using Pab.WebAdmin.Models;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Pab.WebAdmin.Pages.Products
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
        public ProductDto Product { get; set; } = new ProductDto();

        [TempData]
        public string? StatusMessage { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            // 1) Pobierz token JWT z sesji
            var token = HttpContext.Session.GetString("JWToken");
            if (string.IsNullOrEmpty(token))
            {
                return RedirectToPage("/Account/Login");
            }

            // 2) Przygotuj HttpClient
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["ApiBaseUrl"]!);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // 3) Pobierz produkt po ID
            var response = await client.GetAsync($"api/Products/{id}");
            if (!response.IsSuccessStatusCode)
            {
                return RedirectToPage("Index"); // w razie b³êdu wróæ do listy
            }

            var json = await response.Content.ReadAsStringAsync();
            Product = JsonConvert.DeserializeObject<ProductDto>(json)!;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) return Page();

            var token = HttpContext.Session.GetString("JWToken");
            if (string.IsNullOrEmpty(token))
                return RedirectToPage("/Account/Login");

            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["ApiBaseUrl"]!);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // Serializuj zmieniony produkt do JSON i wyœlij PUT
            var json = JsonConvert.SerializeObject(Product);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await client.PutAsync($"api/Products/{Product.Id}", content);

            if (response.IsSuccessStatusCode)
            {
                StatusMessage = "Produkt zosta³ zaktualizowany.";
                return RedirectToPage("Index");
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                return RedirectToPage("/Account/Login");
            }

            // Je¿eli b³¹d
            ModelState.AddModelError(string.Empty, "Wyst¹pi³ problem podczas aktualizacji.");
            return Page();
        }
    }
}
