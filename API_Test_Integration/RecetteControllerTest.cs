using API_Test_Integration.Fixture;
using System.Net.Http.Json;
using SaveursInedites_Jalon2.Domain.DTO.DTOOut;

namespace API_Test_Integration
{
    public class RecetteControllerTest : IntegrationTest
    {
        public RecetteControllerTest(APIWebApplicationFactory webApi) : base(webApi)
        {
        }

        [Fact]
        public async void GetRecettes_Sould_Return_6_Recettes()
        {

            // Arrange
            List<RecetteDTO> recettesExpected = new List<RecetteDTO>()
                {

                new()
                {
                Id = 1,
                Nom = "nom_1",
                TempsPreparation = TimeSpan.FromMinutes(20),
                TempsCuisson = TimeSpan.FromMinutes(15),
                Difficulte = 2,
                Photo = "photo_1",
                Createur = 1
                },
                new()
                 {
                Id = 2,
                Nom = "nom_2",
                TempsPreparation = TimeSpan.FromMinutes(30),
                TempsCuisson = TimeSpan.FromMinutes(25),
                Difficulte = 3,
                Photo = "photo_2",
                Createur = 1
                },
                 new()
                {
                Id = 3,
                Nom = "nom_3",
                TempsPreparation = TimeSpan.FromMinutes(15),
                TempsCuisson = TimeSpan.FromMinutes(00),
                Difficulte = 2,
                Photo = "photo_3",
                Createur = 1
                },
                new()
                {
                Id = 4,
                Nom = "nom_4",
                TempsPreparation = TimeSpan.FromMinutes(30),
                TempsCuisson = TimeSpan.FromMinutes(30),
                Difficulte = 5,
                Photo = "photo_3",
                Createur = 1
                },
                new()
                {
                Id = 5,
                Nom = "nom_5",
                TempsPreparation = TimeSpan.FromMinutes(20),
                TempsCuisson = TimeSpan.FromMinutes(00),
                Difficulte = 2,
                Photo = "photo_3",
                Createur = 1
                },
                 new()
                {
                Id = 6,
                Nom = "nom_6",
                TempsPreparation = TimeSpan.FromMinutes(30),
                TempsCuisson = TimeSpan.FromMinutes(40),
                Difficulte = 3,
                Photo = "photo_3",
                Createur = 1
                },

            };


            // Act
            var list = await httpClient.GetFromJsonAsync<List<RecetteDTO>>("api/recettes");


            // Assert
            Assert.NotNull(list);
            Assert.Equivalent(recettesExpected, list);
        }
    }
}
