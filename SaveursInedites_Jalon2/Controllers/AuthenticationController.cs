using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SaveursInedites_Jalon2.Domain.DTO.DTOIn;
using SaveursInedites_Jalon2.Domain.DTO.DTOOut;
using SaveursInedites_Jalon2.Services;
using SaveursInedites_Jalon2.Domain.BO;

namespace SaveursInedites_Jalon2.Controllers
{
    /// <summary>
    /// Contrôleur pour l'authentification des utilisateurs et la génération de jetons JWT.
    /// </summary>
    [AllowAnonymous]
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IJwtTokenService _jwtTokenService;
        private readonly ISaveursService _saveursService;

        public AuthenticationController(
            IJwtTokenService jwtTokenService,
            ISaveursService saveursService)
        {
            _jwtTokenService = jwtTokenService;
            _saveursService = saveursService;
        }

        /// <summary>
        /// Authentifie un utilisateur et retourne un jeton JWT si les informations sont valides.
        /// </summary>
        [HttpPost("Login")]
        public async Task<IActionResult> Login(
            IValidator<LoginDTO> validator,
            [FromBody] LoginDTO request)
        {
            // Validation des données d'entrée
            validator.ValidateAndThrow(request);

            // 1) Recherche de l'utilisateur par identifiant (username)
            Utilisateur? utilisateur =
                await _saveursService.GetUtilisateurByIdentifiantAsync(request.Username);

            // 2) Si utilisateur inexistant ou mot de passe invalide => 401
            if (utilisateur is null ||
                !BCrypt.Net.BCrypt.Verify(request.Password, utilisateur.Password))
            {
                // 401 Unauthorized
                return Unauthorized("Nom d'utilisateur ou mot de passe incorrect.");
            }

            // 3) Détermination du rôle à partir de Role_id
            string role = utilisateur.Role_id switch
            {
                1 => "Administrateur",
                2 => "Utilisateur",
                _ => "Utilisateur"
            };

            // 4) Génération du token JWT
            var token = _jwtTokenService.GenerateToken(utilisateur.Identifiant, role);

            return Ok(new JwtDTO { Token = token });
        }
    }
}
