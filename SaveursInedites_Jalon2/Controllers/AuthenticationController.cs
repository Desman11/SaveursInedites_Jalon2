using FluentValidation;
// Utilisation de FluentValidation pour valider les DTO d’entrée (LoginDTO).

using Microsoft.AspNetCore.Authorization;
// Fournit les attributs et mécanismes d’autorisation ([Authorize], [AllowAnonymous], rôles, etc.).

using Microsoft.AspNetCore.Mvc;
// Fournit les types nécessaires pour créer un contrôleur Web API (ControllerBase, IActionResult, etc.).

using SaveursInedites_Jalon2.Domain.DTO.DTOIn;
// Namespace des DTO utilisés en entrée (ici LoginDTO).

using SaveursInedites_Jalon2.Domain.DTO.DTOOut;
// Namespace des DTO utilisés en sortie (ici JwtDTO).

using SaveursInedites_Jalon2.Services;
// Accès aux services métiers, notamment IJwtTokenService et ISaveursService.

using SaveursInedites_Jalon2.Domain.BO;
// Accès aux objets métiers (Business Objects), ici Utilisateur.

namespace SaveursInedites_Jalon2.Controllers
// Namespace regroupant les contrôleurs de l’API.
{
    /// <summary>
    /// Contrôleur pour l'authentification des utilisateurs et la génération de jetons JWT.
    /// </summary>
    [AllowAnonymous]
    // Toutes les actions de ce contrôleur sont accessibles sans être authentifié.
    // (On ouvre l’accès pour permettre la connexion.)

    [Route("api/[controller]")]
    // Route de base du contrôleur : "api/authentication" (nom du contrôleur sans "Controller").

    [ApiController]
    // Indique qu’il s’agit d’un contrôleur d’API (validation modèle, binding automatique, etc.).
    public class AuthenticationController : ControllerBase
    // Contrôleur responsable de l’authentification et de la délivrance des tokens JWT.
    {
        private readonly IJwtTokenService _jwtTokenService;
        // Service dédié à la génération des jetons JWT.

        private readonly ISaveursService _saveursService;
        // Service métier pour accéder aux utilisateurs (recherche par identifiant, etc.).

        public AuthenticationController(
            IJwtTokenService jwtTokenService,
            ISaveursService saveursService)
        // Constructeur : les services nécessaires sont injectés via l’IoC container.
        {
            _jwtTokenService = jwtTokenService;
            // Stockage de la référence au service de génération de JWT.

            _saveursService = saveursService;
            // Stockage de la référence au service métier (accès utilisateurs).
        }

        /// <summary>
        /// Authentifie un utilisateur et retourne un jeton JWT si les informations sont valides.
        /// </summary>
        [HttpPost("Login")]
        // Endpoint POST "api/authentication/login".

        public async Task<IActionResult> Login(
            IValidator<LoginDTO> validator,
            // Validateur FluentValidation pour LoginDTO (injecté par le conteneur).

            [FromBody] LoginDTO request)
        // Objet reçu dans le corps de la requête, contenant Username et Password.
        {
            // Validation des données d'entrée
            validator.ValidateAndThrow(request);
            // Valide le DTO. Si des règles échouent, une ValidationException est levée
            // et gérée par le middleware global (renvoi d’erreur 400).

            // 1) Recherche de l'utilisateur par identifiant (username)
            Utilisateur? utilisateur =
                await _saveursService.GetUtilisateurByIdentifiantAsync(request.Username);
            // On interroge la couche service pour retrouver un utilisateur
            // correspondant au nom d’utilisateur saisi.

            // 2) Si utilisateur inexistant ou mot de passe invalide => 401
            if (utilisateur is null ||
                !BCrypt.Net.BCrypt.Verify(request.Password, utilisateur.Password))
            // Si aucun utilisateur n’est trouvé OU si la vérification du mot de passe échoue...
            {
                // 401 Unauthorized
                return Unauthorized("Nom d'utilisateur ou mot de passe incorrect.");
                // On retourne un statut HTTP 401 avec un message générique.
            }

            // 3) Détermination du rôle à partir de Role_id
            string role = utilisateur.Role_id switch
            // On convertit le Role_id (int) en chaîne de rôle exploitée par [Authorize(Roles = ...)].
            {
                1 => "Administrateur",
                2 => "Utilisateur",
                _ => "Utilisateur"
                // Valeur par défaut : "Utilisateur" si Role_id ne correspond pas à un cas connu.
            };

            // 4) Génération du token JWT
            var token = _jwtTokenService.GenerateToken(utilisateur.Identifiant, role);
            // On appelle le service JWT pour générer un token signé,
            // contenant au minimum le username et le rôle dans les claims.

            return Ok(new JwtDTO { Token = token });
            // Retourne 200 OK avec un objet JwtDTO contenant le token au format JSON.
        }
    }
}
