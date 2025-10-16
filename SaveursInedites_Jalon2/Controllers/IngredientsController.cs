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
    public class IngredientsController : ControllerBase
    {
        private readonly ISaveursService _saveursService;

        public IngredientsController(ISaveursService saveursService)
        {
            _saveursService = saveursService;
        }

        [HttpGet()]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetIngredient()
        {
            var ingredients = await _saveursService.GetAllIngredientsAsync();

            IEnumerable<IngredientDTO> response = ingredients.Select(i => new IngredientDTO()
            {
                Id = i.Id,
                Nom = i.Nom,
                });

            return Ok(response);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetIngredientById([FromRoute] int id)
        {
            var ingredient = await _saveursService.GetIngredientsByIdAsync(id);

            if (ingredient is null)
                return NotFound();

            IngredientsDTO response = new()
            {
                Id = ingredient.Id,
                Nom = ingredient.Nom,
              
            };

            return Ok(response);
        }
      IngredientsDTO reponse = new
       {
           Id = ingredients.Id,
           Nom = ingredients.Nom,
         
       };

            return Ok(response);
        }

        [HttpPost()]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateIngredient(IValidator<CreateIngredientsDTO> validator, [FromBody] CreateIngredientsDTO request)
        {
            validator.ValidateAndThrow(request);

            Ingredients ingredients = new()
            {
                Nom = request.Nom,
               
            };

            var newIngredient = await _saveursService.AddIngredientsAsync(ingredients);

            if (newIngredient is null)
                return BadRequest("Invalid ingredient data.");

            IngredientsDTO response = new()
            {
                Id = newIngredients.Id,
                Nom = newIngredients.Nom,

            };

            return CreatedAtAction(nameof(GetIngredientById), new { id = response.Id }, response);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateIngredient(IValidator<UpdateIngredientsDTO> validator, [FromRoute] int id, [FromBody] UpdateIngredientsDTO request)
        {
            validator.ValidateAndThrow(request);

            Ingredients ingredients = new()
            {
                Id = id,
                Nom = request.Nom,
             
            };

            var modifiedIngredients = await _saveursService.ModifyIngredientsAsync(ingredients);

            if (modifiedIngredients is null)
                return BadRequest("Invalid ingredient.");

            IngredientsDTO response = new()
            {
                Id = modifiedIngredients.Id,
                Nom = modifiedIngredients.Nom,

            };

            return Ok(response);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteIngredients([FromRoute] int id)
        {
            var success = await _saveursService.DeleteIngredientsAsync(id);
            return success ? NoContent() : NotFound();
        }
    }
}

