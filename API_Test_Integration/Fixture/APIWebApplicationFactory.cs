using Microsoft.AspNetCore.Hosting;                         // Import des APIs d’hébergement ASP.NET Core
using Microsoft.AspNetCore.Mvc.Testing;                    // Fournit WebApplicationFactory pour les tests d’intégration
using Microsoft.Extensions.Configuration;                  // Gestion de la configuration (appsettings, variables, etc.)
using Microsoft.Extensions.DependencyInjection;            // Gestion de l’injection de dépendances (DI)
using SaveursInedites_Jalon2;                              // Namespace du projet principal (pour Program)
using SaveursInedites_Jalon2.Domain;                       // Namespace contenant IDatabaseSettings et DatabaseSettings

namespace API_Test_Integration.Fixture;                    // Namespace dédié aux fixtures des tests d’intégration

// Classe permettant de créer une instance de l'API en mode test.
// Hérite de WebApplicationFactory<Program> pour réutiliser tout le pipeline ASP.NET Core.
public class APIWebApplicationFactory : WebApplicationFactory<Program>
{
    // Stocke la configuration utilisée pendant les tests (chargée depuis un fichier dédié)
    public IConfiguration Configuration { get; set; }

    // Méthode permettant de modifier la configuration de l’hôte pour les tests
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        // Appel à la version de base pour initialiser le comportement standard de la factory
        base.ConfigureWebHost(builder);

        // Définition d'une configuration spécifique aux tests d’intégration
        builder.ConfigureAppConfiguration(config =>
        {
            // Crée une nouvelle configuration basée sur le fichier appsettings.Integrations.json
            Configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.Integrations.json")   // Ajoute le fichier JSON dédié aux tests
                .Build();                                       // Génère l’objet IConfiguration

            // Ajoute cette configuration au pipeline de configuration de l’hôte web
            config.AddConfiguration(Configuration);
        });

        // Modification de l’enregistrement des services de l'API pour le contexte de test
        builder.ConfigureServices(sc =>
        {
            // Recherche un service existant de type IDatabaseSettings dans la collection des services
            var databaseSettings = sc.FirstOrDefault(service => service is IDatabaseSettings);

            // Supprime le service trouvé (permet de le remplacer par une version test)
            sc.Remove(databaseSettings);

            // Ajoute un singleton IDatabaseSettings configuré selon appsettings.Integrations.json
            sc.AddSingleton<IDatabaseSettings>(sp =>
                new DatabaseSettings()
                {
                    // Récupère la chaîne de connexion PostgreSQL définie dans le fichier d’intégration
                    ConnectionString = Configuration
                        .GetSection("DatabaseSettings")
                        .GetValue<string>("PostgreSQL"),

                    // Récupère le provider à utiliser (souvent PostgreSQL)
                    DatabaseProviderName = Configuration
                        .GetSection("DatabaseSettings")
                        .GetValue<DatabaseProviderName>("DatabaseProviderName")
                });
        });

        // Second appel à la méthode de base (inutile mais sans conséquence)
        base.ConfigureWebHost(builder);
    }
}
