using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SaveursInedites_Jalon2.Services;

namespace SaveursInedites_Jalon2.Controllers
{
    /// <summary>
    /// Contrôleur API pour la gestion des relations entre recettes et ingredients.
    /// Permet d'ajouter, de supprimer et de consulter les relations entre les recettes et les ingredients.
    /// Accessible aux administrateurs.
    /// </summary>
    [Authorize(Roles = "Administrateur")]
    [Route("api/[controller]")]
    [ApiController]
    public class RecettesIngredientsRelationsController : ControllerBase
    {
        private readonly ISaveursService _saveursService;

        /// <summary>
        /// Initialise une nouvelle instance de la classe <see cref="RecettesIngredientsRelationsController"/>.
        /// </summary>
        /// <param name="saveursService">Service de gestion des recttes.</param>
        public RecettesIngredientsRelationsController(ISaveursService saveursService)
        {
            _saveursService = saveursService;
        }

        /// <summary>
        /// Ajoute une relation entre un ingredient et une recette.
        /// </summary>
        /// <param name="idIngredient">Identifiant de l'ingredient.</param>
        /// <param name="idRecette">Identifiant de la recette.</param>
        /// <returns>Code 204 si la relation est ajoutée, 404 si l'ingredient ou la recette n'existe pas.</returns>
        [HttpPost(nameof(GetIngredientsByIdRecette) + "/{idRecette}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> AddRecetteAuthorRelationship([FromRoute] int idIngredient, [FromRoute] int idRecette)
        {
            var success = await _saveursService.AddRecetteIngredientRelationshipAsync(idIngredient, idRecette);
            return success ? NoContent() : NotFound();
        }

        /// <summary>
        /// Supprime une relation entre un ingredient et une recette.
        /// </summary>
        /// <param name="idIngredient">Identifiant de l'ingredient.</param>
        /// <param name="idRecette">Identifiant de la recette.</param>
        /// <returns>Code 204 si la relation est supprimée, 404 si la relation n'existe pas.</returns>
        [HttpDelete(nameof(GetIngredientsByIdRecette) + "/{idRecette}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> RemoverecetteIngredientRelationship([FromRoute] int idIngredient, [FromRoute] int idRecette)
        {
            var success = await _saveursService.RemoveRecetteIngredientRelationshipAsync(idIngredient, idRecette);
            return success ? NoContent() : NotFound();
        }

        /// <summary>
        /// Récupère la liste des recette associés à un ingredient.
        /// </summary>
        /// <param name="idIngredient">Identifiant de l'ingredient.</param>
        /// <returns>Liste des recette liés à l'ingredient.</returns>
        [HttpGet(nameof(GetRecettesByIdIngredient) + "/{idIngredient}")]
        public async Task<IActionResult> GetRecettesByIdIngredient([FromRoute] int idIngredient)
        {
            var response = await _saveursService.GetRecettesByIdIngredientAsync(idIngredient);
            return Ok(response);
        }

        /// <summary>
        /// Récupère la liste des ingredients associés à une recette.
        /// <param name="idRecette">Identifiant d'une recette.</param>
        /// <returns>Liste des ingredients liés a une recette.</returns>
        [HttpGet(nameof(GetIngredientsByIdRecette) + "/{idRecette}")]
        public async Task<IActionResult> GetIngredientsByIdRecette([FromRoute] int idRecette)
        {
            await _saveursService.GetIngredientsByIdRecetteAsync(idRecette);
            return Ok();
        }
        /// <summary>
        /// Supprime toutes les relations d'une recette avec des ingredients.
        /// </summary>
        /// <param name="idRecette">Identifiant d'une recette.</param>
        /// <returns>Code 204 si les relations sont supprimées, 404 si la recette n'existe pas.</returns>
        [HttpDelete(nameof(DeleteRecetteRelations) + "/{idRecette}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteRecetteRelations([FromRoute] int idRecette)
        {
            var success = await _saveursService.DeleteRecetteRelationsAsync(idRecette);
            return success ? NoContent() : NotFound();
        }

        /// <summary>
        /// Supprime toutes les relations d'un ingredient avec des recette.
        /// </summary>
        /// <param name="idIngredient">Identifiant de l'ingredient.</param>
        /// <returns>Code 204 si les relations sont supprimées, 404 si l'ingredient n'existe pas.</returns>
        [HttpDelete(nameof(DeleteIngredientRelations) + "/{idIngredient}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteIngredientRelations([FromRoute] int idIngredient)
        {
            var success = await _saveursService.DeleteIngredientRelationsAsync(idIngredient);
            return success ? NoContent() : NotFound();
        }
    }
}
