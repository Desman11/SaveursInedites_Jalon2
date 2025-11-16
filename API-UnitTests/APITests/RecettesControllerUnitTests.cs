
using Microsoft.AspNetCore.Mvc;
using Moq;
using SaveursInedites_Jalon2.Controllers;
using SaveursInedites_Jalon2.Domain.BO;
using SaveursInedites_Jalon2.Domain.DTO.DTOOut;
using SaveursInedites_Jalon2.Services;

namespace API_UnitTests.APITests                     // Définit le namespace du projet de tests unitaires
{
    public class RecettesControllerUnitTests          // Classe contenant les tests unitaires du contrôleur RecettesController
    {
        // Les commentaires suivants décrivent les différents scénarios de test envisagés :
        // Get Recettes = OK([])              → Le service renvoie une liste vide
        // Get Recettes = OK(List<Recette>)   → Le service renvoie une liste d’éléments
        // Get Recette = Null                 → Le service renvoie null (aucune recette trouvée)

        [Fact]                                        // Attribut de xUnit indiquant qu’il s’agit d’un test unitaire à exécuter
        public async void GetRecettes_Should_Be_OkObjectResult_With_Empty_List() // Nom explicite du test : vérifie qu’une liste vide retourne un OkObjectResult vide
        {
            // ---------- ARRANGE ----------
            ISaveursService saveursService = Mock.Of<ISaveursService>(); // Crée un mock (objet simulé) de l’interface ISaveursService
            Mock.Get(saveursService)                   // Récupère le mock associé à l’objet simulé
    .Setup(s => s.GetAllRecettesAsync())   // Configure le comportement du mock pour la méthode GetAllRecettesAsync()
    .ReturnsAsync(new List<Recette>());    // Cette méthode renverra une liste vide lorsqu’elle sera appelée
            RecettesController recettesController = new RecettesController(saveursService); // Instancie le contrôleur avec le service simulé injecté (dépendance mockée)
            OkObjectResult expectedResult = new OkObjectResult(new List<RecetteDTO>()); // Crée le résultat attendu : une réponse HTTP 200 OK contenant une liste vide de RecetteDTO
            // ---------- ACT ----------
            var actualResult = await recettesController // Appelle la méthode réelle du contrôleur
                .GetRecettes();                         // Exécution de la méthode GetRecettes() (asynchrone)
            // ---------- ASSERT ----------
            Assert.IsType(expectedResult.GetType(), actualResult); // Vérifie que le résultat obtenu est bien du type OkObjectResult (HTTP 200 OK)
            OkObjectResult okObjectActualResult = actualResult as OkObjectResult; // Convertit le résultat obtenu en OkObjectResult pour accéder à sa valeur
            Assert.Empty(okObjectActualResult.Value as IEnumerable<RecetteDTO>); // Vérifie que la liste renvoyée dans la réponse est vide (aucune recette)
        }

        // Attribut xUnit indiquant qu'il s'agit d'un test unitaire
        [Fact]
        public async void GetRecettes_Should_Be_OkObjectResult_With_2_RecettesDTO()
        {
            // ---------- ARRANGE ----------
            // Création d'une première entité Recette avec Id = 1
            var recette1 = new Recette() { Id = 1 };

            // Création d'une deuxième entité Recette avec Id = 2
            var recette2 = new Recette() { Id = 2 };

            // Constitution d'une liste contenant les deux recettes
            var listRecettes = new List<Recette> { recette1, recette2 };

            // Création d'un mock pour le service ISaveursService
            ISaveursService saveursService = Mock.Of<ISaveursService>();

            // Configuration du mock : quand GetAllRecettesAsync est appelé,
            // il doit retourner la liste listRecettes
            Mock.Get(saveursService)
                .Setup(s => s.GetAllRecettesAsync())
                .ReturnsAsync(listRecettes);

            // Instanciation du contrôleur en injectant le service mocké
            RecettesController recettesController = new RecettesController(saveursService);

            // Construction du résultat attendu :
            // - conversion de chaque Recette en RecetteDTO
            // - encapsulation de cette liste de DTO dans un OkObjectResult
            OkObjectResult expectedResult = new OkObjectResult(
                listRecettes.Select(a => new RecetteDTO
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
            // Appel de l'action GetRecettes du contrôleur
            var actualResult = await recettesController.GetRecettes();

            // ---------- ASSERT ----------
            // Vérifie que le type du résultat retourné est bien OkObjectResult
            Assert.IsType(expectedResult.GetType(), actualResult);

            // Cast du résultat en OkObjectResult pour pouvoir accéder à sa Value
            OkObjectResult okObjectActualResult = actualResult as OkObjectResult;

            // Vérifie que le contenu (Value) du OkObjectResult réel
            // est équivalent au contenu attendu (liste de RecetteDTO)
            // Le paramètre 'true' permet une comparaison plus flexible des objets.
            Assert.Equivalent(expectedResult.Value, okObjectActualResult.Value, true);
        }
    }
}





