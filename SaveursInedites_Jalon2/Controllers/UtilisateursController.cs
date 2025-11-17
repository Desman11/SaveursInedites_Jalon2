using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SaveursInedites_Jalon2.Domain.BO;
using SaveursInedites_Jalon2.Domain.DTO.DTOIn;
using SaveursInedites_Jalon2.Domain.DTO.DTOOut;
using SaveursInedites_Jalon2.Services;
using BCrypt.Net;

namespace SaveursInedites_Jalon2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UtilisateursController : ControllerBase
    {
        private readonly ISaveursService _saveursService;

        public UtilisateursController(ISaveursService saveurService)
        {
            _saveursService = saveurService;
        }

        [HttpGet()]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetUtilisateurs()
        {
            var utilisateurs = await _saveursService.GetAllUtilisateursAsync();

            IEnumerable<UtilisateurDTO> response = utilisateurs.Select(a => new UtilisateurDTO()
            {
                Id = a.Id,
                Identifiant = a.Identifiant,
                Email = a.Email,
                Role_id = a.Role_id,
                Password = a.Password

            });

            return Ok(response);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetUtilisateurById([FromRoute] int id)
        {
            var utilisateur = await _saveursService.GetUtilisateurByIdAsync(id);

            if (utilisateur is null)
                return NotFound();

            UtilisateurDTO response = new()
            {
                Id = utilisateur.Id,
                Identifiant = utilisateur.Identifiant,
                Email = utilisateur.Email,
                Role_id = utilisateur.Role_id,
                Password = utilisateur.Password
            };

            return Ok(response);
        }

        [HttpPost()]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateUtilisateur(IValidator<CreateUtilisateurDTO> validator, [FromBody] CreateUtilisateurDTO request)
        {
            validator.ValidateAndThrow(request);

            // Hashage du mot de passe avant de créer l'entité
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(request.Password);

            Utilisateur utilisateur = new()
            {
                Email = request.Email,
                Identifiant = request.Identifiant,
                Password = hashedPassword,
                Role_id = request.Role_id
                
            };

            var newUtilisateur = await _saveursService.AddUtilisateurAsync(utilisateur);

            if (newUtilisateur is null)
                return BadRequest("Invalid utilisateur data.");

            UtilisateurDTO response = new()
            {
                Id = newUtilisateur.Id,
                Identifiant = newUtilisateur.Identifiant,
                Email = newUtilisateur.Email,
                Password = newUtilisateur.Password,
                Role_id = newUtilisateur.Role_id
            };

            return CreatedAtAction(nameof(GetUtilisateurById), new { id = response.Id }, response);
        }

        private object GetUtilisateurById()
        {
            throw new NotImplementedException();
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
                Role_id = request.Role_id   
            };

            var modifiedUtilisateur = await _saveursService.ModifyUtilisateurAsync(utilisateur);

            if (modifiedUtilisateur is null)
                return BadRequest("Invalid utilisateur.");

            UtilisateurDTO response = new()
            {
                Id = modifiedUtilisateur.Id,
                Identifiant = modifiedUtilisateur.Identifiant,
                Email = modifiedUtilisateur.Email,
                Role_id = modifiedUtilisateur.Role_id
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
