using System;
using System.Collections.Generic;
using System.Net.Http.Json;
using System.Threading.Tasks;
using API_Test_Integration.Fixture;
using SaveursInedites_Jalon2.Domain.DTO.DTOOut;
using Xunit;

namespace API_Test_Integration
{
    public class RecetteControllerTest : IntegrationTest
    {
        public RecetteControllerTest(APIWebApplicationFactory webApi) : base(webApi)
        {
        }

        [Fact]
        public async Task GetRecettes_Should_Return_6_Recettes()
        {
            // Arrange
            DownDatabase();
            UpDatabase();
            await Login("admin", "admin");

            var recettesExpected = new List<RecetteDTO>
            {
                new()
                {
                    Id = 1,
                    Nom = "nom_1",
                    TempsPreparation = TimeSpan.Zero,      // '00:00:00' en base
                    TempsCuisson = TimeSpan.Zero,          // '00:00:00' en base
                    Difficulte = 3,                        // comme ton INSERT d’origine
                    Photo = "photo_1",
                    Createur = 0                           // NULL -> 0 dans un int non nullable
                },
                new()
                {
                    Id = 2,
                    Nom = "nom_2",
                    TempsPreparation = TimeSpan.Zero,
                    TempsCuisson = TimeSpan.Zero,
                    Difficulte = 2,
                    Photo = "photo_1",                     // ta base met "photo_1" partout
                    Createur = 0
                },
                new()
                {
                    Id = 3,
                    Nom = "nom_3",
                    TempsPreparation = TimeSpan.Zero,
                    TempsCuisson = TimeSpan.Zero,
                    Difficulte = 2,
                    Photo = "photo_1",
                    Createur = 0
                },
                new()
                {
                    Id = 4,
                    Nom = "nom_4",
                    TempsPreparation = TimeSpan.Zero,
                    TempsCuisson = TimeSpan.Zero,
                    Difficulte = 5,
                    Photo = "photo_1",
                    Createur = 0
                },
                new()
                {
                    Id = 5,
                    Nom = "nom_5",
                    TempsPreparation = TimeSpan.Zero,
                    TempsCuisson = TimeSpan.Zero,
                    Difficulte = 2,
                    Photo = "photo_1",
                    Createur = 0
                },
                new()
                {
                    Id = 6,
                    Nom = "nom_6",
                    TempsPreparation = TimeSpan.Zero,
                    TempsCuisson = TimeSpan.Zero,
                    Difficulte = 3,
                    Photo = "photo_1",
                    Createur = 0
                }
            };

            // Act
            var recettes = await httpClient.GetFromJsonAsync<List<RecetteDTO>>("/api/Recettes/");

            // Assert
            Assert.NotNull(recettes);
            Assert.Equal(6, recettes!.Count);
            Assert.Equivalent(recettesExpected, recettes);

            // Cleanup
            Logout();
        }
    }
}
