using FluentValidation;
// Utilisation de FluentValidation pour valider les DTO en entrée.

using Microsoft.AspNetCore.Authorization;
// Fournit les attributs et mécanismes d’autorisation ([Authorize], rôles, etc.).

using Microsoft.AspNetCore.Mvc;
// Fournit les types pour construire des contrôleurs Web API.

using SaveursInedites_Jalon2.Domain.BO;
// Accès aux objets métiers (Business Objects), ici Ingredient.

using SaveursInedites_Jalon2.Domain.DTO.DTOIn;
// Accès aux DTO utilisés en entrée (CreateIngredientDTO, UpdateIngredientDTO).

using SaveursInedites_Jalon2.Domain.DTO.DTOOut;
// Accès aux DTO utilisés en sortie (IngredientDTO).

using SaveursInedites_Jalon2.Services;
// Accès à l’interface ISaveursService (couche métier).

namespace SaveursInedites_Jalon2.Controllers
// Namespace regroupant les contrôleurs de l’API.
{
    /// <summary>
    /// Contrôleur API pour la gestion des ingredients.
    /// Permet de récupérer, créer, modifier et supprimer des ingredients.
    /// Accessible aux administrateurs et aux utilisateurs.
    /// </summary>
    [Authorize(Roles = "Administrateur,Utilisateur")]
    // Toutes les actions de ce contrôleur sont accessibles uniquement aux utilisateurs
    // authentifiés ayant l’un des rôles "Administrateur" ou "Utilisateur".

    [Route("api/[controller]")]
    // Route de base : "api/ingredients" (nom du contrôleur sans "Controller").

    [ApiController]
    // Indique qu’il s’agit d’un contrôleur d’API (binding, validation automatique, etc.).
    public class IngredientsController : ControllerBase
    // Contrôleur exposant les endpoints pour la ressource Ingredient.
    {
        private readonly ISaveursService _saveursService;
        // Service métier permettant d’accéder aux opérations sur les ingrédients.

        /// <summary>
        /// Initialise une nouvelle instance de la classe <see cref="IngredientsController"/>.
        /// </summary>
        /// <param name="saveursService">Service de gestion des ingredients.</param>
        public IngredientsController(ISaveursService saveursService)
        // Constructeur appelé par l’injection de dépendances.
        {
            _saveursService = saveursService;
            // Stocke l’instance du service pour utilisation dans les actions.
        }

        /// <summary>
        /// Récupère la liste de tous les ingredients.
        /// </summary>
        /// <returns>Une liste d'ingredients.</returns>
        [HttpGet()]
        // Action HTTP GET sur "api/ingredients".

        [ProducesResponseType(StatusCodes.Status200OK)]
        // Indique que l’action peut répondre avec un 200 OK.
        public async Task<IActionResult> GetIngredients()
        // Récupère tous les ingrédients et les retourne en DTO.
        {
            var ingredients = await _saveursService.GetAllIngredientsAsync();
            // Appel à la couche service pour obtenir la liste des ingrédients (BO).

            IEnumerable<IngredientDTO> response = ingredients.Select(i => new IngredientDTO()
            // Projection des objets métier Ingredient vers des DTO IngredientDTO.
            {
                Id = i.Id,
                Nom = i.Nom,
            });

            return Ok(response);
            // Retourne 200 OK avec la liste des DTO sérialisée en JSON.
        }

        /// <summary>
        /// Récupère un ingredient par son identifiant.
        /// </summary>
        /// <param name="id">Identifiant de l'ingredient.</param>
        /// <returns>L'ingredient correspondant à l'identifiant, ou le code 404 si l'ingredient n'existe pas.</returns>
        [HttpGet("{id}")]
        // GET "api/ingredients/{id}".

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        // Peut renvoyer 200 ou 404.
        public async Task<IActionResult> GetIngredientById([FromRoute] int id)
        // Récupère un ingrédient à partir de son id.
        {
            var ingredient = await _saveursService.GetIngredientByIdAsync(id);
            // Appelle la couche service pour trouver l’ingrédient.

            if (ingredient is null)
                return NotFound();
            // Si aucun ingrédient trouvé, renvoie 404 NotFound.

            IngredientDTO response = new()
            // Création du DTO de réponse.
            {
                Id = ingredient.Id,
                Nom = ingredient.Nom,
            };

            return Ok(response);
            // Retourne 200 avec le DTO de l’ingrédient.
        }

        /// <summary>
        /// Crée un nouvel ingredient.
        /// </summary>
        /// <param name="validator">Validateur pour le modèle de création d'un ingredient'.</param>
        /// <param name="request">Données de l'ingredient à créer.</param>
        /// <returns>L'ingredient créée, ou le code 400 en cas d'erreur.
        /// </returns>
        [HttpPost()]
        // POST "api/ingredients".

        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        // Peut renvoyer 201 Created ou 400 BadRequest.
        public async Task<IActionResult> CreateIngredient(
            IValidator<CreateIngredientDTO> validator,
            // Validateur FluentValidation injecté pour CreateIngredientDTO.

            [FromBody] CreateIngredientDTO request)
        // Données d’entrée reçues dans le corps de la requête.
        {
            validator.ValidateAndThrow(request);
            // Valide le DTO et lève une ValidationException si les règles ne sont pas respectées.

            Ingredient ingredient = new()
            // Création de l’objet métier Ingredient à partir du DTO.
            {
                Id = request.Id,
                Nom = request.Nom,
            };

            var newIngredient = await _saveursService.AddIngredientAsync(ingredient);
            // Appel de la couche service pour persister le nouvel ingrédient.

            if (newIngredient is null)
                return BadRequest("Invalid ingredient data.");
            // En cas d’échec, renvoie 400 avec un message d’erreur.

            IngredientDTO response = new()
            // Prépare le DTO de réponse à partir de l’objet métier créé.
            {
                Id = newIngredient.Id,
                Nom = newIngredient.Nom,
            };

            return CreatedAtAction(nameof(GetIngredientById), new { id = response.Id }, response);
            // Retourne 201 Created, avec l’URL pour récupérer l’élément et le DTO en corps de réponse.
        }

        /// <summary>
        /// Met à jour un ingredient existant.
        /// </summary>
        /// <param name="validator">Validateur pour le modèle de mise à jour d'un ingredient'.</param>
        /// <param name="id">Identifiant de l'ingredient à mettre à jour.</param>
        /// <param name="request">Données mises à jour de l'ingredient'.</param>
        /// <returns>L'ingredient mise à jour, ou le code 400 en cas d'erreur.</returns>
        [HttpPut("{id}")]
        // PUT "api/ingredients/{id}".

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        // Peut renvoyer 200 OK ou 400 BadRequest.
        public async Task<IActionResult> UpdateIngredient(
            IValidator<UpdateIngredientDTO> validator,
            // Validateur pour UpdateIngredientDTO.

            [FromRoute] int id,
            // Id dans l’URL (source de vérité attendue).

            [FromBody] UpdateIngredientDTO request)
        // Données mises à jour dans le corps de la requête.
        {
            validator.ValidateAndThrow(request);
            // Valide le DTO de mise à jour.

            Ingredient ingredient = new()
            // Construit l’objet métier Ingredient à partir du DTO.
            {
                Id = request.Id,
                // Ici, on utilise l’Id du DTO (et non celui de la route).

                Nom = request.Nom,
            };

            var modifiedIngredient = await _saveursService.ModifyIngredientAsync(ingredient);
            // Appel à la couche service pour modifier l’ingrédient existant.

            if (modifiedIngredient is null)
                return BadRequest("Invalid ingredient.");
            // Si la mise à jour échoue (par ex. id inexistant), renvoie 400.

            IngredientDTO response = new()
            // Crée le DTO de retour avec les données mises à jour.
            {
                Id = modifiedIngredient.Id,
                Nom = modifiedIngredient.Nom,
            };

            return Ok(response);
            // Retourne 200 OK avec l’ingrédient modifié.
        }

        /// <summary>
        /// Supprime un ingredient par son identifiant.
        /// </summary>
        /// <param name="id">Identifiant de l'ingredient à supprimer.</param>
        /// <returns>
        /// Un code 204 si la suppression a réussi, ou 404 si l'ingredient n'existe pas.
        /// </returns>
        [HttpDelete("{id}")]
        // DELETE "api/ingredients/{id}".

        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        // Peut renvoyer 204 ou 404.
        public async Task<IActionResult> DeleteIngredient([FromRoute] int id)
        // Supprime un ingrédient par son identifiant.
        {
            var success = await _saveursService.DeleteIngredientAsync(id);
            // Appelle la couche service pour effectuer la suppression.

            return success ? NoContent() : NotFound();
            // 204 NoContent si la suppression a réussi, 404 NotFound sinon.
        }
    }
}
