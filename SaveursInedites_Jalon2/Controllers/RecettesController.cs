using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using SaveursInedites_Jalon2.Domain.BO;
using SaveursInedites_Jalon2.Domain.DTO.In;
using SaveursInedites_Jalon2.Domain.DTO.Out;
using SaveursInedites_Jalon2.Services;

namespace SaveursInedites_Jalon2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecettesController : ControllerBase
    {
        private readonly ISaveursService _saveursService;

        public RecettesController(ISaveursService saveursService)
        {
            _saveursService = saveursService;
        }

        [HttpGet()]
        [ProducesResponseType(StatusCodes.Status200OK)]
                      public async Task<IActionResult> GetRecette()
        {
            var recette = await _saveursService.GetAllRecettesAsync();

            IEnumerable<RecetteDTO> response = recette.Select(r => new RecetteDTO()
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

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetRecetteById([FromRoute] int id)
        {
            var recette = await _saveursService.GetRecetteByIdAsync(id);

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

        [HttpPost()]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateRecette(IValidator<CreateRecetteDTO> validator, [FromBody] CreateRecetteDTO request)
        {
            validator.ValidateAndThrow(request);

            Recette recette = new()
            {
                Nom = request.Nom,
                TempsPreparation = request.TempsPreparation,
                TempsCuisson = request.TempsCuisson,
                Difficulte = request.Difficulte,
                Photo = request.Photo,
                Createur = request.Createur
            };

            var newRecette = await _saveursService.AddRecetteAsync(recette);

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

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateRecette(IValidator<UpdateRecetteDTO> validator, [FromRoute] int id, [FromBody] UpdateRecetteDTO request)
        {
            validator.ValidateAndThrow(request);

            Recette recette = new()
            {
                Id = id,
                Nom = request.Nom,
                TempsPreparation = request.TempsPreparation,
                TempsCuisson = request.TempsCuisson,
                Difficulte = request.Difficulte,
                Photo = request.Photo,
                Createur = request.Createur
            };

            var modifiedRecette = await _saveursService.ModifyRecetteAsync(recette);

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

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteRecette([FromRoute] int id)
        {
            var success = await _saveursService.DeleteRecetteAsync(id);
            return success ? NoContent() : NotFound();
        }
    }
}
