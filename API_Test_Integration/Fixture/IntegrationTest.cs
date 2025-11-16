using System.Data;
using System.Net.Http.Json;
using Microsoft.Extensions.Configuration;
using Npgsql;
using SaveursInedites_Jalon2.Domain.DTO.DTOIn;
using SaveursInedites_Jalon2.Domain.DTO.DTOOut;

// Attribut xUnit : désactive l’exécution des tests en parallèle
// (important ici car on manipule une base de données partagée)
[assembly: CollectionBehavior(DisableTestParallelization = true)]

namespace API_Test_Integration.Fixture;

// Classe de base pour tous les tests d’intégration.
// Elle utilise le pattern IClassFixture pour partager le même
// APIWebApplicationFactory entre plusieurs classes de tests.
public abstract class IntegrationTest : IClassFixture<APIWebApplicationFactory>
{
    // Client HTTP utilisé pour appeler l’API pendant les tests
    public HttpClient httpClient { get; set; }

    // Configuration de l’application (appsettings, etc.)
    private readonly IConfiguration _configuration;

    // Constructeur appelé par le framework de tests.
    // Le APIWebApplicationFactory est injecté automatiquement par xUnit.
    public IntegrationTest(APIWebApplicationFactory webApi)
    {
        // Instanciation du HttpClient à partir de la factory de l’API.
        // Ce client va pointer sur l’API de test hébergée en mémoire.
        httpClient = webApi.CreateClient();

        // Récupération de la configuration de l’API de test
        _configuration = webApi.Configuration;

        // Recréation de la base de données avant chaque test (optionnel)
        // DownDatabase();
        // UpDatabase();
    }

    // Méthode utilitaire pour se connecter à l’API avec un login/mot de passe.
    // Elle envoie une requête HTTP POST sur /api/Authentication/Login.
    public async Task Login(string username, string password)
    {
        // Envoi du login/mot de passe au format JSON dans le corps de la requête
        var httpResponse = await httpClient.PostAsJsonAsync<LoginDTO>("api/Authentication/Login", new()
        {
            Username = username,
            Password = password
        });

        // Si la connexion est réussie (code HTTP 2xx)
        if (httpResponse.IsSuccessStatusCode)
        {
            // Désérialisation de la réponse JSON en JwtDTO
            var JwtDTO = await httpResponse.Content.ReadFromJsonAsync<JwtDTO>();

            // Ajout du token JWT dans les en-têtes Authorization du HttpClient
            // pour que les prochains appels soient authentifiés.
            httpClient.DefaultRequestHeaders.Authorization = new("Bearer", JwtDTO.Token);
        }
        else
        {
            // En cas d’échec, le test échoue explicitement
            Assert.Fail("Impossible de se connecter avec {username} , {password}");
        }
    }

    // Méthode utilitaire pour “se déconnecter” :
    // on supprime le header Authorization du HttpClient.
    public void Logout()
    {
        httpClient.DefaultRequestHeaders.Authorization = null;
    }

    // Recréation de la base (schéma + données) à partir d’un script SQL
    public void UpDatabase()
    {
        // Récupération de la chaîne de connexion PostgreSQL dans la configuration
        var StringConnection = _configuration
            .GetSection("DatabaseSettings")
            .GetValue<string>("PostgreSQL");

        // Ouverture d’une connexion ADO.NET vers PostgreSQL
        IDbConnection con = new NpgsqlConnection(StringConnection);
        con.Open();

        // Lecture du fichier SQL contenant le script de création
        string requeteSQL = File.ReadAllText("CreateDatabase.sql");

        // Création de la commande SQL
        var commande = con.CreateCommand();
        commande.CommandText = requeteSQL;

        // Exécution du script (création du schéma, tables, données, etc.)
        commande.ExecuteNonQuery();

        // Libération de la connexion
        con.Dispose();
    }

    // Suppression du schéma public dans PostgreSQL (reset complet de la base)
    public void DownDatabase()
    {
        // Récupération de la chaîne de connexion PostgreSQL dans la configuration
        var StringConnection = _configuration
            .GetSection("DatabaseSettings")
            .GetValue<string>("PostgreSQL");

        // Utilisation d’un using pour s’assurer de la libération de la connexion
        using (IDbConnection con = new NpgsqlConnection(StringConnection))
        {
            con.Open();

            // Requête SQL qui supprime le schéma public et tout ce qu’il contient
            string requeteSQL = "DROP SCHEMA if exists public cascade;";

            var commande = con.CreateCommand();
            commande.CommandText = requeteSQL;

            // Exécution de la suppression du schéma
            commande.ExecuteNonQuery();
        }
    }
}
