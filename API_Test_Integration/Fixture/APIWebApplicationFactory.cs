using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using SaveursInedites_Jalon2;

namespace API_Test_Integration.Fixture;

public class APIWebApplicationFactory : WebApplicationFactory<Program>
{
    public IConfiguration Configuration { get; set; }
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        base.ConfigureWebHost(builder);

        //appsettings.integration.json
        builder.ConfigureAppConfiguration(config =>
        {
            Configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.Integrations.json")
                .Build();
            config.AddConfiguration(Configuration);

        });

    }
}
