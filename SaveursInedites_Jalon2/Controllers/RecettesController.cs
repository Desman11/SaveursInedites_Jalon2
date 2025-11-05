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
    /// Contrôleur API pour la gestion des recettes.
    /// Permet de récupérer, créer, modifier et supprimer des recettes.
    /// Accessible aux administrateurs et aux utilisateurs.
    /// </summary>
    [Authorize(Roles = "Administrateur,Utilisateur")]
    [Route("api/[controller]")]
    [ApiController]
    public class RecettesController : ControllerBase
    {
        private readonly ISaveursService _recetteService;

        /// <summary>
        /// Initialise une nouvelle instance de la classe <see cref="RecettesController"/>.
        /// </summary>
        /// <param name="recetteService">Service de gestion des recettes.</param>
        public RecettesController(ISaveursService recetteService)
        {
            _recetteService = recetteService;
        }

        /// <summary>
        /// Récupère la liste de tous les recettes.
        /// </summary>
        /// <returns>Une liste de recettes.</returns>
        [HttpGet()]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetRecettes()
        {
            var recettes = await _recetteService.GetAllRecettesAsync();

            IEnumerable<RecetteDTO> response = recettes.Select(r => new RecetteDTO()
            {
                Id = r.Id,
            Nom = r.Nom,

                TempsPreparation = r.TempsPreparation,

                TempsCuisson = r.TempsCuisson,

                Difficulte = r.Difficulte,

                Photo = r.Photo,

                Createur = r.Createur
            });

            return Ok(response);
        }

        /// <summary>
        /// Récupère une recette par son identifiant.
        /// </summary>
        /// <param name="id">Identifiant de la recette.</param>
        /// <returns>La recette correspondant à l'identifiant, ou le code 404 si la recette n'existe pas.</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetRecetteById([FromRoute] int id)
        {
            var recette = await _recetteService.GetRecetteByIdAsync(id);

            if (recette is null)
                return NotFound();

            RecetteDTO response = new()
            {
                Id = recette.Id,
                Nom = recette.Nom,

                TempsPreparation = recette.TempsPreparation,

                TempsCuisson = recette.TempsCuisson,

                Difficulte = recette.Difficulte,

                Photo = recette.Photo,

                Createur = recette.Createur
            };

            return Ok(response);
        }

        /// <summary>
        /// Crée une nouvelle recette.
        /// </summary>
        /// <param name="validator">Validateur pour le modèle de création de recette.</param>
        /// <param name="request">Données de la recette à créer.</param>
        /// <returns>La recette créée, ou le code 400 en cas d'erreur.
        /// </returns>
        [HttpPost()]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateRecette(IValidator<CreateRecetteDTO> validator, [FromBody] CreateRecetteDTO request)
        {
            validator.ValidateAndThrow(request);

            Recette recette      = new()
            {
                Id = request.Id,
                Nom = request.Nom,

                TempsPreparation = request.TempsPreparation,

                TempsCuisson = request.TempsCuisson,

                Difficulte = request.Difficulte,

                Photo = request.Photo,

                Createur = request.Createur
            };

            var newRecette = await _recetteService.AddRecetteAsync(recette);

            if (newRecette is null)
                return BadRequest("Invalid recette data.");

            RecetteDTO response = new()
            {
                Id = newRecette.Id,
                Nom = newRecette.Nom,

                TempsPreparation = newRecette.TempsPreparation,

                TempsCuisson = newRecette.TempsCuisson,

                Difficulte = newRecette.Difficulte,

                Photo = newRecette.Photo,

                Createur = newRecette.Createur
            };

            return CreatedAtAction(nameof(GetRecetteById), new { id = response.Id }, response);
        }

        /// <summary>
        /// Met à jour une recette existante.
        /// </summary>
        /// <param name="validator">Validateur pour le modèle de mise à jour de recette.</param>
        /// <param name="id">Identifiant de la recette à mettre à jour.</param>
        /// <param name="request">Données mises à jour de la recette.</param>
        /// <returns>La recette mise à jour, ou le code 400 en cas d'erreur.</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateRecette(IValidator<UpdateRecetteDTO> validator, [FromRoute] int id, [FromBody] UpdateRecetteDTO request)
        {
            validator.ValidateAndThrow(request);

            Recette recette = new()
            {
                Id = request.Id,
                Nom = request.Nom,

                TempsPreparation = request.TempsPreparation,

                TempsCuisson = request.TempsCuisson,

                Difficulte = request.Difficulte,

                Photo = request.Photo,

                Createur = request.Createur
            };

            var modifiedRecette = await _recetteService.ModifyRecetteAsync(recette);

            if (modifiedRecette is null)
                return BadRequest("Invalid recette.");

            RecetteDTO response = new()
            {
                Id = modifiedRecette.Id,
                Nom = modifiedRecette.Nom,

                TempsPreparation = modifiedRecette.TempsPreparation,

                TempsCuisson = modifiedRecette.TempsCuisson,

                Difficulte = modifiedRecette.Difficulte,

                Photo = modifiedRecette.Photo,

                Createur = modifiedRecette.Createur
            };

            return Ok(response);
        }

        /// <summary>
        /// Supprime une recette par son identifiant.
        /// </summary>
        /// <param name="id">Identifiant de la recette à supprimer.</param>
        /// <returns>
        /// Un code 204 si la suppression a réussi, ou 404 si la recette n'existe pas.
        /// </returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteRecette([FromRoute] int id)
        {
            var success = await _recetteService.DeleteRecetteAsync(id);
            return success ? NoContent() : NotFound();
        }
    }
}