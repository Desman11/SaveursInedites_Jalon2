using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SaveursInedites_Jalon2.Services;

namespace SaveursInedites_Jalon2.Controllers
{

    /// <summary>
    /// Contrôleur API pour la gestion des relations entre recettes et ingrédients.
    /// Permet d'ajouter, de supprimer et de consulter les relations entre les recettes et les ingrédients.
    /// Accessible aux administrateurs.
    /// </summary>
    [Authorize(Roles = "Administrateur")]
    [Route("api/[controller]")]
    [ApiController]
    public class RecettesIngredientsRelationsController : ControllerBase
    {
        private readonly ISaveursService _recetteService;

        /// <summary>
        /// Initialise une nouvelle instance de la classe <see cref="RecettesIngredientsRelationsController"/>.
        /// </summary>
        /// <param name="recetteService">Service de gestion des recettes.</param>
        public RecettesIngredientsRelationsController(ISaveursService recetteService)
        {
            _recetteService = recetteService;
        }

        /// <summary>
        /// Ajoute une relation entre une recette et un ingrédient.
        /// </summary>
        /// <param name="idRecette">Identifiant de la recette.</param>
        /// <param name="idIngredient">Identifiant de l'ingrédient.</param>
        /// <returns>Code 204 si la relation est ajoutée, 404 si la recette ou l'ingrédient n'existe pas.</returns>
        [HttpPost(nameof(AddRecetteIngredientRelationship) + "/{idRecette}/{idIngredient}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> AddRecetteIngredientRelationship([FromRoute] int idRecette, [FromRoute] int idIngredient)
        {
            var success = await _recetteService.AddRecetteIngredientRelationshipAsync(idRecette, idIngredient);
            return success ? NoContent() : NotFound();
        }

        /// <summary>
        /// Supprime une relation entre une recette et un ingrédient.
        /// </summary>
        /// <param name="idRecette">Identifiant de la recette.</param>
        /// <param name="idIngredient">Identifiant de l'ingrédient.</param>
        /// <returns>Code 204 si la relation est supprimée, 404 si la relation n'existe pas.</returns>
        [HttpDelete(nameof(RemoveRecetteIngredientRelationship) + "/{idRecette}/{idIngredient}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> RemoveRecetteIngredientRelationship([FromRoute] int idRecette, [FromRoute] int idIngredient)
        {
            var success = await _recetteService.RemoveRecetteIngredientRelationshipAsync(idRecette, idIngredient);
            return success ? NoContent() : NotFound();
        }

        /// <summary>
        /// Récupère la liste des recettes associées à un ingrédient.
        /// </summary>
        /// <param name="idIngredient">Identifiant de l'ingrédient.</param>
        /// <returns>Liste des recettes liées à l'ingrédient.</returns>
        [HttpGet(nameof(GetRecettesByIdIngredient) + "/{idIngredient}")]
        public async Task<IActionResult> GetRecettesByIdIngredient([FromRoute] int idIngredient)
        {
            var response = await _recetteService.GetRecettesByIdIngredientAsync(idIngredient);
            return Ok(response);
        }

        /// <summary>
        /// Récupère la liste des ingrédients associés à une recette.
        /// </summary>
        /// <param name="idRecette">Identifiant de la recette.</param>
        /// <returns>Liste des ingrédients liés à la recette.</returns>
        [HttpGet(nameof(GetIngredientsByIdRecette) + "/{idRecette}")]
        public async Task<IActionResult> GetIngredientsByIdRecette([FromRoute] int idRecette)
        {
            var response = await _recetteService.GetIngredientsByIdRecetteAsync(idRecette);
            return Ok(response);
        }

        /// <summary>
        /// Supprime toutes les relations d'une recette avec des ingrédients.
        /// </summary>
        /// <param name="idRecette">Identifiant de la recette.</param>
        /// <returns>Code 204 si les relations sont supprimées, 404 si la recette n'existe pas.</returns>
        [HttpDelete(nameof(DeleteRecetteRelations) + "/{idRecette}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteRecetteRelations([FromRoute] int idRecette)
        {
            var success = await _recetteService.DeleteRecetteRelationsAsync(idRecette);
            return success ? NoContent() : NotFound();
        }

        /// <summary>
        /// Supprime toutes les relations d'un ingrédient avec des recettes.
        /// </summary>
        /// <param name="idIngredient">Identifiant de l'ingrédient.</param>
        /// <returns>Code 204 si les relations sont supprimées, 404 si l'ingrédient n'existe pas.</returns>
        [HttpDelete(nameof(DeleteIngredientRelations) + "/{idIngredient}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteIngredientRelations([FromRoute] int idIngredient)
        {
            var success = await _recetteService.DeleteIngredientRelationsAsync(idIngredient);
            return success ? NoContent() : NotFound();
        }
    }
}

