using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using Pab.WebAdmin.Models;
using System.Net.Http.Headers;
using System.Text;

namespace Pab.WebAdmin.Pages.Orders
{
    public class CreateModel : PageModel
    {
        private readonly IHttpClientFactory _http;
        public CreateModel(IHttpClientFactory http) => _http = http;

        [BindProperty]
        public CreateOrderRequest Input { get; set; } = new()
        {
            Items = new List<CreateOrderItem> { new() }  // zaczynamy z jedn¹ pozycj¹
        };

        public void OnGet() { }

        public async Task<IActionResult> OnPostAsync()
        {
            var client = _http.CreateClient("PabApiClient");
            var payload = JsonConvert.SerializeObject(Input);
            var content = new StringContent(payload, Encoding.UTF8, "application/json");
            var res = await client.PostAsync("api/Orders", content);
            if (res.IsSuccessStatusCode)
                return RedirectToPage("Index");

            ModelState.AddModelError("", "Nie uda³o siê zapisaæ zamówienia");
            return Page();
        }
    }
}
