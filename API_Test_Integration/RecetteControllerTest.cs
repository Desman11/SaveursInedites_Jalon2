using System;
using System.Collections.Generic;
using System.Net.Http.Json;
using System.Threading.Tasks;
using API_Test_Integration.Fixture;
using SaveursInedites_Jalon2.Domain.DTO.DTOOut;
using Xunit;

namespace API_Test_Integration
{
    // Classe de tests d’intégration pour le contrôleur Recettes
    public class RecetteControllerTest : IntegrationTest
    {
        // Le constructeur reçoit la factory d’API et la passe à la classe de base IntegrationTest
        public RecetteControllerTest(APIWebApplicationFactory webApi) : base(webApi)
        {
        }

        // Test : la route GET /api/Recettes/ doit retourner 6 recettes
        [Fact]
        public async Task GetRecettes_Should_Return_6_Recettes()
        {
            // ---------- ARRANGE ----------
            // On remet la base dans un état propre (reset complet du schéma public)
            DownDatabase();

            // On recrée le schéma + données de test (les 6 recettes du script SQL)
            UpDatabase();

            // On se connecte avec un utilisateur ayant les droits (admin/admin)
            await Login("admin", "admin");

            // Construction de la liste de recettes attendues,
            // conforme aux données insérées dans CreateDatabase.sql
            var recettesExpected = new List<RecetteDTO>
            {
                new()
                {
                    Id = 1,
                    Nom = "nom_1",
                    TempsPreparation = TimeSpan.Zero,      // correspond à '00:00:00' en base
                    TempsCuisson = TimeSpan.Zero,          // correspond à '00:00:00' en base
                    Difficulte = 3,                        // comme l’INSERT d’origine
                    Photo = "photo_1",
                    Createur = 0                           // NULL en base mappé à 0 dans le DTO
                },
                new()
                {
                    Id = 2,
                    Nom = "nom_2",
                    TempsPreparation = TimeSpan.Zero,
                    TempsCuisson = TimeSpan.Zero,
                    Difficulte = 2,
                    Photo = "photo_1",                     // même photo pour toutes les recettes
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

            // ---------- ACT ----------
            // Appel réel de l’API sur l’endpoint GET /api/Recettes/
            // Le résultat JSON est désérialisé en List<RecetteDTO>
            var recettes = await httpClient.GetFromJsonAsync<List<RecetteDTO>>("/api/Recettes/");

            // ---------- ASSERT ----------
            // On vérifie que la réponse n’est pas nulle
            Assert.NotNull(recettes);

            // On vérifie qu’on a bien 6 éléments dans la liste
            Assert.Equal(6, recettes!.Count);

            // On vérifie que le contenu retourné est équivalent à la liste attendue
            // (mêmes propriétés et mêmes valeurs pour chaque RecetteDTO)
            Assert.Equivalent(recettesExpected, recettes);

            // ---------- CLEANUP ----------
            // On supprime le token d’authentification du HttpClient
            Logout();
        }
    }
}
