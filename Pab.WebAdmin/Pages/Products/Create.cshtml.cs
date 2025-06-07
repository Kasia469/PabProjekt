using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using Pab.WebAdmin.Models;
using System.Net.Http.Headers;

namespace Pab.WebAdmin.Pages.Products
{
    public class CreateModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        public CreateModel(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }

        [BindProperty]
        public ProductDto NewProduct { get; set; } = new ProductDto();

        public void OnGet()
        {
            // Wyświetla formularz (puste NewProduct)
        }

        public async Task<IActionResult> OnPostAsync()
        {
            // Sprawdź token
            var token = HttpContext.Session.GetString("JWToken");
            if (string.IsNullOrEmpty(token))
            {
                return RedirectToPage("/Account/Login");
            }

            // Przygotuj HttpClient
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["ApiBaseUrl"]!);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // Serializuj nowy produkt
            var json = JsonConvert.SerializeObject(new
            {
                name = NewProduct.Name,
                price = NewProduct.Price,
                stock = NewProduct.Stock
            });
            var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

            // Wyślij POST /api/Products
            var response = await client.PostAsync("api/Products", content);
            if (!response.IsSuccessStatusCode)
            {
                ModelState.AddModelError(string.Empty, "Nie udało się dodać produktu.");
                return Page();
            }

            // Jeśli OK – wróć do listy produktów
            return RedirectToPage("/Products/Index");
        }
    }
}
