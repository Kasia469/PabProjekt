using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Pab.WebAdmin.Models;

namespace Pab.WebAdmin.Pages.Orders
{
    public class IndexModel : PageModel
    {
        private readonly IHttpClientFactory _http;
        private readonly IConfiguration _conf;
        public List<OrderDto> Orders { get; set; } = new();

        public IndexModel(IHttpClientFactory http, IConfiguration conf)
        {
            _http = http;
            _conf = conf;
        }

        public async Task OnGetAsync()
        {
            var client = _http.CreateClient("PabApiClient");
            var res = await client.GetAsync("api/Orders");
            if (res.IsSuccessStatusCode)
            {
                var json = await res.Content.ReadAsStringAsync();
                Orders = JsonConvert.DeserializeObject<List<OrderDto>>(json)!;
            }
        }
    }
}
