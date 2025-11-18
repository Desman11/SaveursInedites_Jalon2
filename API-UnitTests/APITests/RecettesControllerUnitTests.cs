using Microsoft.AspNetCore.Mvc;                                   // Import des fonctionnalités MVC (dont OkObjectResult)
using Moq;                                                        // Import de la bibliothèque Moq pour créer des mocks
using SaveursInedites_Jalon2.Controllers;                         // Import du namespace contenant le contrôleur RecettesController
using SaveursInedites_Jalon2.Domain.BO;                           // Import des objets métier (BO), dont Recette
using SaveursInedites_Jalon2.Domain.DTO.DTOOut;                   // Import des DTO envoyés au client (RecetteDTO)
using SaveursInedites_Jalon2.Services;                            // Import des services, dont ISaveursService

namespace API_UnitTests.APITests                                  // Définit le namespace des tests unitaires de l’API
{
    public class RecettesControllerUnitTests                      // Déclare une classe regroupant les tests unitaires du contrôleur RecettesController
    {
        // Commentaires décrivant les scénarios de test visés :
        // - Liste vide => 200 OK + []
        // - Liste avec éléments => 200 OK + liste remplie
        // - Cas où aucune recette n’est trouvée

        [Fact]                                                    // Indique à xUnit qu’il s’agit d’un test
        public async void GetRecettes_Should_Be_OkObjectResult_With_Empty_List()   // Nom du test indiquant le comportement attendu
        {
            // ---------- ARRANGE ----------
            ISaveursService saveursService = Mock.Of<ISaveursService>();          // Création d’un mock automatique de l’interface ISaveursService

            Mock.Get(saveursService)                                             // Récupère le mock interne lié à l’instance simulée
                .Setup(s => s.GetAllRecettesAsync())                             // Configure le mock : interception de l’appel GetAllRecettesAsync()
                .ReturnsAsync(new List<Recette>());                               // Le mock renverra une liste vide

            RecettesController recettesController = new RecettesController(saveursService); // Instancie le contrôleur avec le service mocké injecté

            OkObjectResult expectedResult = new OkObjectResult(new List<RecetteDTO>());     // Prépare un résultat HTTP 200 attendu contenant une liste de DTO vide

            // ---------- ACT ----------
            var actualResult = await recettesController.GetRecettes();            // Appelle réellement l’action GetRecettes() du contrôleur

            // ---------- ASSERT ----------
            Assert.IsType(expectedResult.GetType(), actualResult);                // Valide que la réponse renvoyée est bien un OkObjectResult

            OkObjectResult okObjectActualResult = actualResult as OkObjectResult; // Convertit le résultat pour inspecter sa Value

            Assert.Empty(okObjectActualResult.Value as IEnumerable<RecetteDTO>);  // Vérifie que la liste renvoyée dans l’objet OkObjectResult est vide
        }

        // Attribut xUnit indiquant qu’il s’agit d’un test unitaire
        [Fact]
        public async void GetRecettes_Should_Be_OkObjectResult_With_2_RecettesDTO()
        {
            // ---------- ARRANGE ----------
            var recette1 = new Recette() { Id = 1 };                              // Instancie une première Recette avec Id = 1
            var recette2 = new Recette() { Id = 2 };                              // Instancie une seconde Recette avec Id = 2

            var listRecettes = new List<Recette> { recette1, recette2 };          // Construit une liste contenant les deux recettes

            ISaveursService saveursService = Mock.Of<ISaveursService>();          // Crée un mock pour ISaveursService

            Mock.Get(saveursService)                                              // Récupère le mock
                .Setup(s => s.GetAllRecettesAsync())                              // Configure la méthode interceptée
                .ReturnsAsync(listRecettes);                                      // Retourne listRecettes lors de l’appel réel

            RecettesController recettesController = new RecettesController(saveursService); // Instancie le contrôleur avec le mock injecté

            OkObjectResult expectedResult = new OkObjectResult(                   // Construit le résultat attendu
                listRecettes.Select(a => new RecetteDTO                           // Convertit chaque BO Recette en DTO RecetteDTO
                {
                    Id = a.Id,
                    Nom = a.Nom,
                    TempsPreparation = a.TempsPreparation,
                    TempsCuisson = a.TempsCuisson,
                    Difficulte = a.Difficulte,
                    Photo = a.Photo,
                    Createur = a.Createur
                })
            );

            // ---------- ACT ----------
            var actualResult = await recettesController.GetRecettes();            // Appelle l’action réelle du contrôleur

            // ---------- ASSERT ----------
            Assert.IsType(expectedResult.GetType(), actualResult);                // Vérifie que la réponse est un OkObjectResult

            OkObjectResult okObjectActualResult = actualResult as OkObjectResult; // Cast pour accéder à Value

            Assert.Equivalent(expectedResult.Value, okObjectActualResult.Value, true);
            // Compare le contenu retourné par le contrôleur au contenu attendu
            // Le troisième paramètre active une comparaison flexible (propriétés équivalentes)
        }
    }
}
