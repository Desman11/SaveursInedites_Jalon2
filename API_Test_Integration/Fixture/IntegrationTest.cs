using System.Net.Http.Json;
using SaveursInedites_Jalon2.Domain.DTO.DTOIn;
using SaveursInedites_Jalon2.Domain.DTO.DTOOut;

[assembly: CollectionBehavior(DisableTestParallelization = true)]

namespace API_Test_Integration.Fixture;

public abstract class IntegrationTest : IClassFixture<APIWebApplicationFactory>
{
    public HttpClient httpClient { get; set; }

    public IntegrationTest(APIWebApplicationFactory webApi)
    {
        // Crée un client HTTP pour interagir avec l'API web
        httpClient = webApi.CreateClient();

        //stratégie pour fixer la base de données de test
        //Dapper
        //drop bd si existe
        //relancer jeu de données
    }

    public async Task Login(string username, string password)
    {
        var httpResponse = await httpClient.PostAsJsonAsync<LoginDTO>("api/Authification/login", new()
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


    public void Logout(string username, string password)
    {
        httpClient.DefaultRequestHeaders.Authorization = null;
    }
}


