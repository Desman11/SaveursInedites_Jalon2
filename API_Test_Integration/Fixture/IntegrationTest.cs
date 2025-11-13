using System.Data;
using System.Net.Http.Json;
using Microsoft.Extensions.Configuration;
using Npgsql;
using SaveursInedites_Jalon2.Domain.DTO.DTOIn;
using SaveursInedites_Jalon2.Domain.DTO.DTOOut;

[assembly: CollectionBehavior(DisableTestParallelization = true)]

namespace API_Test_Integration.Fixture;

public abstract class IntegrationTest : IClassFixture<APIWebApplicationFactory>
{

    public HttpClient httpClient { get; set; }
    private readonly IConfiguration _configuration;
    public IntegrationTest(APIWebApplicationFactory webApi)

    {
        // instancier le client
        httpClient = webApi.CreateClient();
        _configuration = webApi.Configuration;

        //Recréation de la base de données avant chaque test
        //DownDatabase();
        //UpDatabase();
    }

    public async Task Login(string username, string password)
    {
        var httpResponse = await httpClient.PostAsJsonAsync<LoginDTO>("api/Authentication/Login", new()
        {
            Username = username,
            Password = password
        });

        if (httpResponse.IsSuccessStatusCode)
        {
           var JwtDTO = await httpResponse.Content.ReadFromJsonAsync<JwtDTO>();
            httpClient.DefaultRequestHeaders.Authorization = new("Bearer", JwtDTO.Token);
        }
        else
        {
            Assert.Fail("Impossible de se connecter avec {username} , {password}");
        }
    }


    public void Logout()
    {
        httpClient.DefaultRequestHeaders.Authorization = null;
    }

  
    public void UpDatabase()
    {
        var StringConnection = _configuration.GetSection("DatabaseSettings").GetValue<string>("PostgreSQL");
        IDbConnection con = new NpgsqlConnection(StringConnection);

        con.Open();
        string requeteSQL = File.ReadAllText("CreateDatabase.sql");
        var commande = con.CreateCommand();
        commande.CommandText = requeteSQL;
        commande.ExecuteNonQuery();
        con.Dispose();
    }

    public void DownDatabase()
    {
        var StringConnection = _configuration.GetSection("DatabaseSettings").GetValue<string>("PostgreSQL");
        using (IDbConnection con = new NpgsqlConnection(StringConnection))
        {
            con.Open();
            string requeteSQL = "DROP SCHEMA if exists public cascade;";
            var commande = con.CreateCommand();
            commande.CommandText = requeteSQL;
            commande.ExecuteNonQuery();
        }
    }
}

