using Microsoft.AspNetCore.Authorization;
// Fournit les attributs et mécanismes d’autorisation (ex : [Authorize]).

using Microsoft.AspNetCore.Mvc;
// Fournit les types et attributs pour construire un contrôleur Web API.

using SaveursInedites_Jalon2.Services;
// Permet d’accéder à l’interface ISaveursService, couche métier.

namespace SaveursInedites_Jalon2.Controllers
// Namespace regroupant les contrôleurs de l’API.
{
    /// <summary>
    /// Gestion des relations Recette ↔ Ingrédient.
    /// Toutes les opérations sont réservées au rôle Administrateur.
    /// </summary>
    [Authorize(Roles = "Administrateur")]
    // Toutes les actions de ce contrôleur sont réservées aux utilisateurs
    // ayant le rôle "Administrateur".

    [ApiController]
    // Indique qu’il s’agit d’un contrôleur d’API (validation automatique du modèle, binding, etc.).

    [Route("api")]
    // Route de base du contrôleur : toutes les routes commenceront par "api".
    public class RecettesIngredientsRelationsController : ControllerBase
    // Contrôleur API pour gérer les relations entre recettes et ingrédients.
    {
        private readonly ISaveursService _saveursService;
        // Service métier injecté, qui encapsule la logique de gestion des recettes/ingrédients.

        public RecettesIngredientsRelationsController(ISaveursService saveursService)
        // Constructeur recevant le service métier via l’injection de dépendances.
        {
            _saveursService = saveursService;
            // Stocke le service dans le champ privé pour l’utiliser dans les actions.
        }

        // --------------------------------------------------------------------
        // Écriture (Admin) : ajouter / supprimer un lien recette↔ingrédient
        // --------------------------------------------------------------------

        /// <summary>Ajoute un lien entre une recette et un ingrédient.</summary>
        /// <returns>204 si ajouté, 404 si recette ou ingrédient introuvable.</returns>
        [HttpPost("recettes/{idRecette:int}/ingredients/{idIngredient:int}")]
        // Route POST : "api/recettes/{idRecette}/ingredients/{idIngredient}"

        [ProducesResponseType(StatusCodes.Status204NoContent)]
        // Indique que l’action peut retourner 204 (No Content) en cas de succès.

        [ProducesResponseType(StatusCodes.Status404NotFound)]
        // Indique que l’action peut retourner 404 si la recette ou l’ingrédient n’existe pas.
        public async Task<IActionResult> AddRecetteIngredientRelationship(
            [FromRoute] int idRecette,
            [FromRoute] int idIngredient)
        // Ajoute une relation (lien) entre la recette et l’ingrédient spécifiés dans l’URL.
        {
            var ok = await _saveursService.AddRecetteIngredientRelationshipAsync(idIngredient, idRecette);
            // Appelle la couche service pour créer la relation entre l’ingrédient et la recette.

            return ok ? NoContent() : NotFound();
            // Si l’ajout réussit : 204 NoContent, sinon 404 NotFound.
        }

        /// <summary>Supprime un lien entre une recette et un ingrédient.</summary>
        /// <returns>204 si supprimé, 404 si le lien n’existe pas.</returns>
        [HttpDelete("recettes/{idRecette:int}/ingredients/{idIngredient:int}")]
        // Route DELETE : "api/recettes/{idRecette}/ingredients/{idIngredient}"

        [ProducesResponseType(StatusCodes.Status204NoContent)]
        // 204 si la relation a bien été supprimée.

        [ProducesResponseType(StatusCodes.Status404NotFound)]
        // 404 si aucune relation n’a été trouvée à supprimer.
        public async Task<IActionResult> RemoveRecetteIngredientRelationship(
            [FromRoute] int idRecette,
            [FromRoute] int idIngredient)
        // Supprime une relation entre la recette et l’ingrédient fournis.
        {
            var ok = await _saveursService.RemoveRecetteIngredientRelationshipAsync(idIngredient, idRecette);
            // Appelle la couche service pour retirer la relation.

            return ok ? NoContent() : NotFound();
            // 204 si succès, 404 sinon.
        }

        // --------------------------------------------------------------------
        // Lecture : listes d’associations
        // --------------------------------------------------------------------

        /// <summary>Liste des recettes associées à un ingrédient.</summary>
        [HttpGet("ingredients/{idIngredient:int}/recettes")]
        // Route GET : "api/ingredients/{idIngredient}/recettes"

        [ProducesResponseType(StatusCodes.Status200OK)]
        // 200 avec la liste des recettes en cas de succès.
        public async Task<IActionResult> GetRecettesByIdIngredient([FromRoute] int idIngredient)
        // Récupère toutes les recettes qui utilisent l’ingrédient donné.
        {
            var recettes = await _saveursService.GetRecettesByIdIngredientAsync(idIngredient);
            // Récupère les recettes associées via la couche service.

            return Ok(recettes);
            // Retourne 200 avec la liste des recettes en JSON.
        }

        /// <summary>Liste des ingrédients associés à une recette.</summary>
        [HttpGet("recettes/{idRecette:int}/ingredients")]
        // Route GET : "api/recettes/{idRecette}/ingredients"

        [ProducesResponseType(StatusCodes.Status200OK)]
        // 200 avec la liste des ingrédients.
        public async Task<IActionResult> GetIngredientsByIdRecette([FromRoute] int idRecette)
        // Récupère tous les ingrédients associés à une recette donnée.
        {
            var ingredients = await _saveursService.GetIngredientsByIdRecetteAsync(idRecette);
            // Demande à la couche service les ingrédients liés à la recette.

            return Ok(ingredients);
            // Retourne 200 avec la liste des ingrédients en JSON.
        }

        // --------------------------------------------------------------------
        // Suppressions en masse
        // --------------------------------------------------------------------

        /// <summary>Supprime toutes les relations d’une recette.</summary>
        /// <returns>204 si supprimées, 404 si la recette n’existe pas.</returns>
        [HttpDelete("recettes/{idRecette:int}/ingredients")]
        // Route DELETE : "api/recettes/{idRecette}/ingredients"

        [ProducesResponseType(StatusCodes.Status204NoContent)]
        // 204 si toutes les relations ont été supprimées (ou inexistantes mais traitées).

        [ProducesResponseType(StatusCodes.Status404NotFound)]
        // 404 si la recette ciblée n’existe pas (selon la logique implémentée en BLL/DAL).
        public async Task<IActionResult> DeleteRecetteRelations([FromRoute] int idRecette)
        // Supprime toutes les relations entre cette recette et ses ingrédients.
        {
            var ok = await _saveursService.DeleteRecetteRelationsAsync(idRecette);
            // Appelle la couche service pour effacer toutes les liaisons de la recette.

            return ok ? NoContent() : NotFound();
            // 204 si succès, 404 sinon.
        }

        /// <summary>Supprime toutes les relations d’un ingrédient.</summary>
        /// <returns>204 si supprimées, 404 si l’ingrédient n’existe pas.</returns>
        [HttpDelete("ingredients/{idIngredient:int}/recettes")]
        // Route DELETE : "api/ingredients/{idIngredient}/recettes"

        [ProducesResponseType(StatusCodes.Status204NoContent)]
        // 204 si les relations ont été supprimées.

        [ProducesResponseType(StatusCodes.Status404NotFound)]
        // 404 si l’ingrédient ciblé n’existe pas (ou selon la logique service).
        public async Task<IActionResult> DeleteIngredientRelations([FromRoute] int idIngredient)
        // Supprime toutes les relations entre cet ingrédient et les recettes.
        {
            var ok = await _saveursService.DeleteIngredientRelationsAsync(idIngredient);
            // Appelle la couche service pour effacer toutes les liaisons de l’ingrédient.

            return ok ? NoContent() : NotFound();
            // 204 si succès, 404 sinon.
        }
    }
}
