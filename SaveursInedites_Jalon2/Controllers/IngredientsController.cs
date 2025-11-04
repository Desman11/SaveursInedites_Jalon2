using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SaveursInedites_Jalon2.Domain.BO;
using SaveursInedites_Jalon2.Domain.DTO.DTOIn;
using SaveursInedites_Jalon2.Domain.DTO.DTOOut;
using SaveursInedites_Jalon2.Services;

namespace SaveursInedites_Jalon2.Controllers
{
    /// <summary>
    /// Contrôleur API pour la gestion des ingredients.
    /// Permet de récupérer, créer, modifier et supprimer des ingredients.
    /// Accessible aux administrateurs et aux utilisateurs.
    /// </summary>
    [Authorize(Roles = "Administrateur,Utilisateur")]
    [Route("api/[controller]")]
    [ApiController]
    public class IngredientsController : ControllerBase
    {
        private readonly ISaveursService _saveursService;

        /// <summary>
        /// Initialise une nouvelle instance de la classe <see cref="IngredientsController"/>.
        /// </summary>
        /// <param name="saveursService">Service de gestion des ingredients.</param>
        public IngredientsController(ISaveursService saveursService)
        {
            _saveursService = saveursService;
        }

        /// <summary>
        /// Récupère la liste de tous les ingredients.
        /// </summary>
        /// <returns>Une liste d'ingredients.</returns>
        [HttpGet()]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetIngredients()
        {
            var ingredients = await _saveursService.GetAllIngredientsAsync();

            IEnumerable<IngredientDTO> response = ingredients.Select(i => new IngredientDTO()
            {
                Id = i.Id,
                Nom = i.Nom,
            
            });

            return Ok(response);
        }

        /// <summary>
        /// Récupère un ingredient par son identifiant.
        /// </summary>
        /// <param name="id">Identifiant de l'ingredient.</param>
        /// <returns>L'ingredient correspondant à l'identifiant, ou le code 404 si l'ingredient n'existe pas.</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetIngredientById([FromRoute] int id)
        {
            var ingredient = await _saveursService.GetIngredientByIdAsync(id);

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
        /// Crée un nouvel ingredient.
        /// </summary>
        /// <param name="validator">Validateur pour le modèle de création d'un ingredient'.</param>
        /// <param name="request">Données de l'ingredient à créer.</param>
        /// <returns>L'ingredient créée, ou le code 400 en cas d'erreur.
        /// </returns>
        [HttpPost()]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateIngredient(IValidator<CreateIngredientDTO> validator, [FromBody] CreateIngredientDTO request)
        {
            validator.ValidateAndThrow(request);

            Ingredient ingredient = new()
            {
                Id = request.Id,
                Nom = request.Nom,
            
            };

            var newIngredient = await _saveursService.AddIngredientAsync(ingredient);

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
        /// <param name="validator">Validateur pour le modèle de mise à jour d'un ingredient'.</param>
        /// <param name="id">Identifiant de l'ingredient à mettre à jour.</param>
        /// <param name="request">Données mises à jour de l'ingredient'.</param>
        /// <returns>L'ingredient mise à jour, ou le code 400 en cas d'erreur.</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateIngredient(IValidator<UpdateIngredientDTO> validator, [FromRoute] int id, [FromBody] UpdateIngredientDTO request)
        {
            validator.ValidateAndThrow(request);

            Ingredient ingredient = new()
            {
                Id = request.Id,
                Nom = request.Nom,

               };

            var modifiedIngredient = await _saveursService.ModifyIngredientAsync(ingredient);

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
        /// Supprime un ingredient par son identifiant.
        /// </summary>
        /// <param name="id">Identifiant de l'ingredient à supprimer.</param>
        /// <returns>
        /// Un code 204 si la suppression a réussi, ou 404 si l'ingredient n'existe pas.
        /// </returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteIngredient([FromRoute] int id)
        {
            var success = await _saveursService.DeleteIngredientAsync(id);
            return success ? NoContent() : NotFound();
        }
    }
}