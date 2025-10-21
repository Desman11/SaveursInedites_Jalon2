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
    public class UtilisateursController : ControllerBase
    {
        private readonly ISaveursService _saveursService;

        public UtilisateursController(ISaveursService biblioService)
        {
            _saveursService = biblioService;
        }

        [HttpGet()]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAuthors()
        {
            var authors = await _saveursService.GetAllUtilisateursAsync();

            IEnumerable<UtilisateurDTO> response = authors.Select(a => new UtilisateurDTO()
            {
                Id = a.Id,
                Identifiant = a.Identifiant,
                Email = a.Email,
              
                Role_id = a.Role_id
            });

            return Ok(response);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAuthorById([FromRoute] int id)
        {
            var utilisateur = await _saveursService.GetUtilisateurByIdAsync(id);

            if (utilisateur is null)
                return NotFound();

            UtilisateurDTO response = new()
            {
                Id = utilisateur.Id,
                Identifiant = utilisateur.Identifiant,
                Email = utilisateur.Email,
               
                Role = utilisateur.Role
            };

            return Ok(response);
        }

        [HttpPost()]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateAuthor(IValidator<CreateUtilisateurDTO> validator, [FromBody] CreateUtilisateurDTO request)
        {
            validator.ValidateAndThrow(request);

            Utilisateur utilisateur = new()
            {
                Email = request.Email,
                Identifiant = request.Identifiant,
                Password = request.Password,
                Role = request.Role
            };

            var newUtilisateur = await _saveursService.AddUtilisateurAsync(utilisateur);

            if (newUtilisateur is null)
                return BadRequest("Invalid author data.");

            UtilisateurDTO response = new()
            {
                Id = newUtilisateur.Id,
                Identifiant = newUtilisateur.Identifiant,
                Email = newUtilisateur.Email,
                
                Role = newUtilisateur.Role
            };

            return CreatedAtAction(nameof(GetUtilisateurById), new { id = response.Id }, response);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateUtilisateur(IValidator<UpdateUtilisateurDTO> validator, [FromRoute] int id, [FromBody] UpdateUtilisateurDTO request)
        {
            validator.ValidateAndThrow(request);

            Utilisateur utilisateur = new()
            {

                Id = id,
                Identifiant = request.Identifiant,
                Email = request.Email,
                Password = request.Password,
                Role = request.Role
            };

            var modifiedUtilisateur = await _saveursService.ModifyUtilisateurAsync(utilisateur);

            if (modifiedUtilisateur is null)
                return BadRequest("Invalid utilisateur.");

            UtilisateurDTO response = new()
            {
                Id = modifiedUtilisateur.Id,
                Identifiant = modifiedUtilisateur.Identifiant,
                Email = modifiedUtilisateur.Email,
               
                Role = modifiedUtilisateur.Role
            };

            return Ok(response);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteUtilisateur([FromRoute] int id)
        {
            var success = await _saveursService.DeleteUtilisateurAsync(id);
            return success ? NoContent() : NotFound();
        }
    }
}
