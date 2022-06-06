using Microsoft.AspNetCore.Hosting;

[assembly: HostingStartup(typeof(ExchangeSecureTexts.Areas.Identity.IdentityHostingStartup))]
namespace ExchangeSecureTexts.Areas.Identity;

public class IdentityHostingStartup : IHostingStartup
{
    public void Configure(IWebHostBuilder builder)
    {
        builder.ConfigureServices((context, services) => {
        });
    }
}