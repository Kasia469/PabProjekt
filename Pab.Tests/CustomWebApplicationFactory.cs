using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Hosting;

namespace Pab.Tests
{
    public class CustomWebApplicationFactory<TProgram>
      : WebApplicationFactory<TProgram> where TProgram : class
    {
        protected override IHostBuilder CreateHostBuilder()
          => Host.CreateDefaultBuilder()
                 .ConfigureWebHostDefaults(webBuilder =>
                   webBuilder.UseStartup<TProgram>());
    }
}
