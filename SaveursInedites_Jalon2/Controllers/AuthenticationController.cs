using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SaveursInedites_Jalon2.Domain.DTO.DTOIn;
using SaveursInedites_Jalon2.Domain.DTO.DTOOut;
using SaveursInedites_Jalon2.Services;
using BCrypt.Net;
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

        /// <summary>
        /// Initialise une nouvelle instance du contrôleur <see cref="AuthenticationController"/>.
        /// </summary>
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
            // Ici, on considère que LoginDTO.Username correspond à la colonne "identifiant"
            Utilisateur? utilisateur = await _saveursService
                .GetUtilisateurByIdentifiantAsync(request.Username);

            if (utilisateur is null)
                throw new UnauthorizedAccessException("Nom d'utilisateur ou mot de passe incorrect.");

            // 2) Vérification du mot de passe (hashé avec BCrypt à la création)
            bool passwordValide = BCrypt.Net.BCrypt.Verify(request.Password, utilisateur.Password);
            if (!passwordValide)
                throw new UnauthorizedAccessException("Nom d'utilisateur ou mot de passe incorrect.");

            // 3) Détermination du rôle à partir de Role_id (adapter selon ta table rôles)
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
