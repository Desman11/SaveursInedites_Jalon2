using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SaveursInedites_Jalon2;
using SaveursInedites_Jalon2.Domain;

namespace API_Test_Integration.Fixture;

// Classe de factory pour héberger l'API dans le cadre des tests d’intégration.
// Elle hérite de WebApplicationFactory<Program>, ce qui permet de démarrer
// le même pipeline qu’en production, mais dans un contexte de test.
public class APIWebApplicationFactory : WebApplicationFactory<Program>
{
    // Propriété qui va contenir la configuration utilisée pour les tests
    public IConfiguration Configuration { get; set; }

    // Méthode appelée pour configurer l’hôte web utilisé par les tests
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        // Appel de l’implémentation de base
        base.ConfigureWebHost(builder);

        // Configuration de l’application pour les tests :
        // on charge un fichier appsettings spécifique aux tests d’intégration.
        builder.ConfigureAppConfiguration(config =>
        {
            // Construction d’un IConfiguration à partir du fichier JSON
            Configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.Integrations.json") // fichier de config dédié aux tests d’intégration
                .Build();

            // On ajoute cette configuration au pipeline de config de l’hôte
            config.AddConfiguration(Configuration);
        });

        // IOC:
        // serviceCollection => phase de configuration (on peut ajouter / retirer des services)
        // serviceProvider  => phase d’exécution (lecture seule, services déjà construits)

        builder.ConfigureServices(sc =>
        {
            // Récupérer dans la collection de services l’enregistrement existant
            // lié à IDatabaseSettings (singleton configuré dans l’API réelle)
            var databaseSettings = sc.FirstOrDefault(service => service is IDatabaseSettings);

            // Supprimer ce service pour le remplacer par une configuration adaptée aux tests
            sc.Remove(databaseSettings);

            // Ajouter un nouveau singleton IDatabaseSettings configuré avec
            // la chaîne de connexion et le provider définis dans appsettings.Integrations.json
            sc.AddSingleton<IDatabaseSettings>(sp =>
                new DatabaseSettings()
                {
                    // Récupération de la chaîne de connexion PostgreSQL dédiée aux tests
                    ConnectionString = Configuration
                        .GetSection("DatabaseSettings")
                        .GetValue<string>("PostgreSQL"),

                    // Récupération du provider de base de données (PostgreSQL, etc.)
                    DatabaseProviderName = Configuration
                        .GetSection("DatabaseSettings")
                        .GetValue<DatabaseProviderName>("DatabaseProviderName")
                });
        });

        // Appel de base (ici redondant, mais n’impacte pas le fonctionnement)
        base.ConfigureWebHost(builder);
    }
}
