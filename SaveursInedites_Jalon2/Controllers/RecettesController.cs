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
    /// API Recettes : lecture (authentifiés), création/modification/suppression (administrateurs).
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json", "application/problem+json")]
    [Authorize] // par défaut: authentifié (User ou Admin)
    public class RecettesController : ControllerBase
    {
        private readonly ISaveursService _recetteService;

        public RecettesController(ISaveursService recetteService)
        {
            _recetteService = recetteService;
        }

        /// <summary>Liste toutes les recettes.</summary>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<RecetteDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetRecettes()
        {
            var recettes = await _recetteService.GetAllRecettesAsync();

            var response = recettes.Select(r => new RecetteDTO
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

        /// <summary>Récupère une recette par id.</summary>
        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(RecetteDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetRecetteById([FromRoute] int id)
        {
            var r = await _recetteService.GetRecetteByIdAsync(id);
            if (r is null)
                return Problem(statusCode: 404, title: "Introuvable", detail: $"Recette {id} inexistante.");

            var response = new RecetteDTO
            {
                Id = r.Id,
                Nom = r.Nom,
                TempsPreparation = r.TempsPreparation,
                TempsCuisson = r.TempsCuisson,
                Difficulte = r.Difficulte,
                Photo = r.Photo,
                Createur = r.Createur
            };
            return Ok(response);
        }

        /// <summary>Crée une nouvelle recette. (Admin)</summary>
        [HttpPost]
        [Authorize(Roles = "Administrateur")]
        [ProducesResponseType(typeof(RecetteDTO), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> CreateRecette(
            [FromServices] IValidator<CreateRecetteDTO> validator,
            [FromBody] CreateRecetteDTO request)
        {
            validator.ValidateAndThrow(request);

            // L'Id est géré par la base; on ignore request.Id s'il existe
            var entity = new Recette
            {
                Nom = request.Nom,
                TempsPreparation = request.TempsPreparation,
                TempsCuisson = request.TempsCuisson,
                Difficulte = request.Difficulte,
                Photo = request.Photo,
                Createur = request.Createur
            };

            var created = await _recetteService.AddRecetteAsync(entity);
            if (created is null)
                return Problem(statusCode: 400, title: "Requête invalide", detail: "Création impossible.");

            var response = new RecetteDTO
            {
                Id = created.Id,
                Nom = created.Nom,
                TempsPreparation = created.TempsPreparation,
                TempsCuisson = created.TempsCuisson,
                Difficulte = created.Difficulte,
                Photo = created.Photo,
                Createur = created.Createur
            };

            return CreatedAtAction(nameof(GetRecetteById), new { id = response.Id }, response);
        }

        /// <summary>Met à jour une recette. (Admin)</summary>
        [HttpPut("{id:int}")]
        [Authorize(Roles = "Administrateur")]
        [ProducesResponseType(typeof(RecetteDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateRecette(
            [FromServices] IValidator<UpdateRecetteDTO> validator,
            [FromRoute] int id,
            [FromBody] UpdateRecetteDTO request)
        {
            validator.ValidateAndThrow(request);

            var existing = await _recetteService.GetRecetteByIdAsync(id);
            if (existing is null)
                return Problem(statusCode: 404, title: "Introuvable", detail: $"Recette {id} inexistante.");

            var toUpdate = new Recette
            {
                Id = id, // id route = source de vérité
                Nom = request.Nom,
                TempsPreparation = request.TempsPreparation,
                TempsCuisson = request.TempsCuisson,
                Difficulte = request.Difficulte,
                Photo = request.Photo,
                Createur = request.Createur
            };

            var updated = await _recetteService.ModifyRecetteAsync(toUpdate);
            if (updated is null)
                return Problem(statusCode: 400, title: "Requête invalide", detail: "Mise à jour impossible.");

            var response = new RecetteDTO
            {
                Id = updated.Id,
                Nom = updated.Nom,
                TempsPreparation = updated.TempsPreparation,
                TempsCuisson = updated.TempsCuisson,
                Difficulte = updated.Difficulte,
                Photo = updated.Photo,
                Createur = updated.Createur
            };

            return Ok(response);
        }

        /// <summary>Supprime une recette. (Admin)</summary>
        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Administrateur")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteRecette([FromRoute] int id)
        {
            var ok = await _recetteService.DeleteRecetteAsync(id);
            if (!ok)
                return Problem(statusCode: 404, title: "Introuvable", detail: $"Recette {id} inexistante.");
            return NoContent();
        }
    }
}
