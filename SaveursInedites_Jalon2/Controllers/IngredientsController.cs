using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SaveursInedites_Jalon2.Domain.BO;
using SaveursInedites_Jalon2.Domain.DTO.In;
using SaveursInedites_Jalon2.Domain.DTO.Out;
using SaveursInedites_Jalon2.Services;

namespace SaveursInedites_Jalon2.Controllers
{
    /// <summary>
    /// Contrôleur API pour la gestion des Ingredients.
    /// Permet de récupérer, créer, modifier et supprimer des ingrédients.
    /// Accessible aux administrateurs et aux utilisateurs.
    /// </summary>
    [Authorize(Roles = "Administrateur,Utilisateur")]
    [Route("api/[controller]")]
    [ApiController]
    public class IngredientsController : ControllerBase
    {
        private readonly ISaveursService _ingredientService;

        /// <summary>
        /// Initialise une nouvelle instance de la classe <see cref="IngredientsController"/>.
        /// </summary>
        /// <param name="ingredientService">Service de gestion des ingrédients.</param>
        public IngredientsController(ISaveursService ingredientService)
        {
            _ingredientService = ingredientService;
        }

        /// <summary>
        /// Récupère la liste de tous les ingrédients.
        /// </summary>
        /// <returns>Une liste d'ingrédients.</returns>
        [HttpGet()]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetIngredients()
        {
            var ingredients = await _ingredientService.GetAllIngredientsAsync();

            IEnumerable<IngredientDTO> response = ingredients.Select(i => new IngredientDTO()
            {
                Id = i.Id,
                Nom = i.Nom,
               
            });

            return Ok(response);
         
        }

        /// <summary>
        /// Récupère d'un ingredient par son identifiant.
        /// </summary>
        /// <param name="id">Identifiant de l'ingrédient.</param>
        /// <returns>L'ingrédient correspondant à l'identifiant, ou le code 404 si l'ingrédient n'existe pas.</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetIngredientById([FromRoute] int id)
        {
            var ingredient = await _ingredientService.GetIngredientByIdAsync(id);

            if (ingredient is null)
                return NotFound();

            IngredientDTO response = new()
            {
                Id = ingredient.Id,
                Nom = ingredient.Nom,
            
            };

            return Ok(response);
        }

        /// <summary>
        /// Crée un nouveau livre.
        /// </summary>
        /// <param name="validator">Validateur pour le modèle de création de livre.</param>
        /// <param name="request">Données du livre à créer.</param>
        /// <returns>Le livre créé, ou le code 400 en cas d'erreur.
        /// </returns>
        [HttpPost()]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateIngredient(IValidator<CreateIngredientDTO> validator, [FromBody] CreateIngredientDTO request)
        {
            validator.ValidateAndThrow(request);

            Ingredient ingredient = new()
            {
                Nom = request.Nom,
            
            };

            var newIngredient = await _ingredientService.AddIngredientAsync(ingredient);

            if (newIngredient is null)
                return BadRequest("Invalid ingredient data.");

            IngredientDTO response = new()
            {
                Id = newIngredient.Id,
                Nom = newIngredient.Nom,
             
            };

            return CreatedAtAction(nameof(GetIngredientById), new { id = response.Id }, response);
        }

        /// <summary>
        /// Met à jour un ingredient existant.
        /// </summary>
        /// <param name="validator">Validateur pour le modèle de mise à jour d'ingredient.</param>
        /// <param name="id">Identifiant de l'ingredient à mettre à jour.</param>
        /// <param name="request">Données mises à jour de l'ingredient.</param>
        /// <returns>L'ingredient mis à jour, ou le code 400 en cas d'erreur.</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateIngredient(IValidator<UpdateIngredientDTO> validator, [FromRoute] int id, [FromBody] UpdateIngredientDTO request)
        {
            validator.ValidateAndThrow(request);

            Ingredient ingredient = new()
            {
                Id = id,
                Nom = request.Nom,
         
            };

            var modifiedIngredient = await _ingredientService.ModifyIngredientAsync(ingredient);

            if (modifiedIngredient is null)
                return BadRequest("Invalid ingredient.");

            IngredientDTO response = new()
            {
                Id = modifiedIngredient.Id,
                Nom = modifiedIngredient.Nom,
              
            };

            return Ok(response);
        }

        /// <summary>
        /// Supprime un livre par son identifiant.
        /// </summary>
        /// <param name="id">Identifiant du livre à supprimer.</param>
        /// <returns>
        /// Un code 204 si la suppression a réussi, ou 404 si l'auteur n'existe pas.
        /// </returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteIngredient([FromRoute] int id)
        {
            var success = await _ingredientService.DeleteIngredientAsync(id);
            return success ? NoContent() : NotFound();
        }
    }
}