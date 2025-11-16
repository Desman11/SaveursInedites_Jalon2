using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SaveursInedites_Jalon2.Services;

namespace SaveursInedites_Jalon2.Controllers
{
    /// <summary>
    /// Gestion des relations Recette ↔ Ingrédient.
    /// Toutes les opérations sont réservées au rôle Administrateur.
    /// </summary>
    [Authorize(Roles = "Administrateur")]
    [ApiController]
    [Route("api")]
    public class RecettesIngredientsRelationsController : ControllerBase
    {
        private readonly ISaveursService _saveursService;

        public RecettesIngredientsRelationsController(ISaveursService saveursService)
        {
            _saveursService = saveursService;
        }

        // --------------------------------------------------------------------
        // Écriture (Admin) : ajouter / supprimer un lien recette↔ingrédient
        // --------------------------------------------------------------------

        /// <summary>Ajoute un lien entre une recette et un ingrédient.</summary>
        /// <returns>204 si ajouté, 404 si recette ou ingrédient introuvable.</returns>
        [HttpPost("recettes/{idRecette:int}/ingredients/{idIngredient:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> AddRecetteIngredientRelationship(
            [FromRoute] int idRecette,
            [FromRoute] int idIngredient)
        {
            var ok = await _saveursService.AddRecetteIngredientRelationshipAsync(idIngredient, idRecette);
            return ok ? NoContent() : NotFound();
        }

        /// <summary>Supprime un lien entre une recette et un ingrédient.</summary>
        /// <returns>204 si supprimé, 404 si le lien n’existe pas.</returns>
        [HttpDelete("recettes/{idRecette:int}/ingredients/{idIngredient:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> RemoveRecetteIngredientRelationship(
            [FromRoute] int idRecette,
            [FromRoute] int idIngredient)
        {
            var ok = await _saveursService.RemoveRecetteIngredientRelationshipAsync(idIngredient, idRecette);
            return ok ? NoContent() : NotFound();
        }

        // --------------------------------------------------------------------
        // Lecture : listes d’associations
        // --------------------------------------------------------------------

        /// <summary>Liste des recettes associées à un ingrédient.</summary>
        [HttpGet("ingredients/{idIngredient:int}/recettes")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetRecettesByIdIngredient([FromRoute] int idIngredient)
        {
            var recettes = await _saveursService.GetRecettesByIdIngredientAsync(idIngredient);
            return Ok(recettes);
        }

        /// <summary>Liste des ingrédients associés à une recette.</summary>
        [HttpGet("recettes/{idRecette:int}/ingredients")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetIngredientsByIdRecette([FromRoute] int idRecette)
        {
            var ingredients = await _saveursService.GetIngredientsByIdRecetteAsync(idRecette);
            return Ok(ingredients);
        }

        // --------------------------------------------------------------------
        // Suppressions en masse
        // --------------------------------------------------------------------

        /// <summary>Supprime toutes les relations d’une recette.</summary>
        /// <returns>204 si supprimées, 404 si la recette n’existe pas.</returns>
        [HttpDelete("recettes/{idRecette:int}/ingredients")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteRecetteRelations([FromRoute] int idRecette)
        {
            var ok = await _saveursService.DeleteRecetteRelationsAsync(idRecette);
            return ok ? NoContent() : NotFound();
        }

        /// <summary>Supprime toutes les relations d’un ingrédient.</summary>
        /// <returns>204 si supprimées, 404 si l’ingrédient n’existe pas.</returns>
        [HttpDelete("ingredients/{idIngredient:int}/recettes")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteIngredientRelations([FromRoute] int idIngredient)
        {
            var ok = await _saveursService.DeleteIngredientRelationsAsync(idIngredient);
            return ok ? NoContent() : NotFound();
        }
    }
}
