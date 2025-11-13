using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SaveursInedites_Jalon2;
using SaveursInedites_Jalon2.Domain;


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
        //IOC:
        //serviceCollection => read&Write
        //serviceProvider => readOnly

        builder.ConfigureServices(sc =>
        {
            //Supprimer le singleton dbsettings
            var databaseSettings = sc.FirstOrDefault(service => service is IDatabaseSettings);
            sc.Remove(databaseSettings);

            //Ajouter le nouveau singleton
            sc.AddSingleton<IDatabaseSettings>(sp =>
            new DatabaseSettings()
            {
                ConnectionString = Configuration.GetSection("DatabaseSettings").GetValue<string>("PostgreSQL"),
                DatabaseProviderName = Configuration.GetSection("DatabaseSettings").GetValue<DatabaseProviderName>("DatabaseProviderName")
            });


        });
        base.ConfigureWebHost(builder);
    }
}
