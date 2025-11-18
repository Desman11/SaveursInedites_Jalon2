using FluentValidation;
// Utilise FluentValidation pour valider les DTO en entrée.

using Microsoft.AspNetCore.Authorization;
// Fournit les attributs et types liés à l’autorisation (rôles, [Authorize], etc.).

using Microsoft.AspNetCore.Mvc;
// Fournit les types de base pour construire des contrôleurs Web API.

using SaveursInedites_Jalon2.Domain.BO;
// Accès aux objets métiers (BO) comme Utilisateur.

using SaveursInedites_Jalon2.Domain.DTO.DTOIn;
// Accès aux DTO utilisés en entrée (CreateUtilisateurDTO, UpdateUtilisateurDTO, etc.).

using SaveursInedites_Jalon2.Domain.DTO.DTOOut;
// Accès aux DTO utilisés en sortie (UtilisateurDTO).

using SaveursInedites_Jalon2.Services;
// Accès à l’interface ISaveursService, couche métier.

using BCrypt.Net;
// Référence la bibliothèque BCrypt pour le hashage des mots de passe.

namespace SaveursInedites_Jalon2.Controllers
// Namespace qui regroupe les contrôleurs de l’API.
{
    [Route("api/[controller]")]
    // Définit la route de base : "api/utilisateurs" (nom du contrôleur sans "Controller").

    [ApiController]
    // Indique qu’il s’agit d’un contrôleur API (gestion automatique de la validation modèle, etc.).
    public class UtilisateursController : ControllerBase
    // Contrôleur Web API pour gérer les utilisateurs.
    {
        private readonly ISaveursService _saveursService;
        // Service métier injecté, utilisé pour manipuler les utilisateurs.

        public UtilisateursController(ISaveursService saveurService)
        // Constructeur recevant le service via l’injection de dépendances.
        {
            _saveursService = saveurService;
            // Stocke la référence au service pour l’utiliser dans les actions.
        }

        [HttpGet()]
        // Action HTTP GET sur "api/utilisateurs".

        [ProducesResponseType(StatusCodes.Status200OK)]
        // Indique que l’action peut retourner un statut 200 avec un corps de réponse.
        public async Task<IActionResult> GetUtilisateurs()
        // Récupère et renvoie la liste de tous les utilisateurs.
        {
            var utilisateurs = await _saveursService.GetAllUtilisateursAsync();
            // Appelle la couche service pour obtenir tous les utilisateurs métiers.

            IEnumerable<UtilisateurDTO> response = utilisateurs.Select(a => new UtilisateurDTO()
            // Projette les objets métier Utilisateur en DTO UtilisateurDTO.
            {
                Id = a.Id,
                Identifiant = a.Identifiant,
                Email = a.Email,
                Role_id = a.Role_id,
                Password = a.Password
                // Attention : ici le mot de passe est renvoyé au client (ce qui n’est en général pas souhaitable).
            });

            return Ok(response);
            // Retourne un statut 200 avec la liste des utilisateurs en JSON.
        }

        [HttpGet("{id}")]
        // Action HTTP GET sur "api/utilisateurs/{id}" pour récupérer un utilisateur précis.

        [ProducesResponseType(StatusCodes.Status200OK)]
        // Peut retourner 200 avec un utilisateur.

        [ProducesResponseType(StatusCodes.Status404NotFound)]
        // Peut retourner 404 si l’utilisateur n’existe pas.
        public async Task<IActionResult> GetUtilisateurById([FromRoute] int id)
        // Récupère un utilisateur par son identifiant.
        {
            var utilisateur = await _saveursService.GetUtilisateurByIdAsync(id);
            // Demande à la couche service l’utilisateur correspondant à l’id.

            if (utilisateur is null)
                return NotFound();
            // Si aucun utilisateur trouvé, renvoie 404.

            UtilisateurDTO response = new()
            // Crée le DTO de sortie à partir de l’objet métier.
            {
                Id = utilisateur.Id,
                Identifiant = utilisateur.Identifiant,
                Email = utilisateur.Email,
                Role_id = utilisateur.Role_id,
                Password = utilisateur.Password
            };

            return Ok(response);
            // Retourne 200 avec le DTO utilisateur.
        }

        [HttpPost()]
        // Action HTTP POST sur "api/utilisateurs" pour créer un nouvel utilisateur.

        [ProducesResponseType(StatusCodes.Status201Created)]
        // Peut retourner 201 avec la ressource créée.

        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        // Peut retourner 400 si les données envoyées sont invalides.
        public async Task<IActionResult> CreateUtilisateur(
            IValidator<CreateUtilisateurDTO> validator,
            [FromBody] CreateUtilisateurDTO request)
        // Crée un nouvel utilisateur à partir des données reçues dans le corps de la requête.
        {
            validator.ValidateAndThrow(request);
            // Valide le DTO d’entrée. Si invalide, lance une ValidationException (gérée par le middleware global).

            // Hashage du mot de passe avant de créer l'entité
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(request.Password);
            // Chiffre le mot de passe clair pour ne jamais le stocker en texte brut.

            Utilisateur utilisateur = new()
            // Construit l’objet métier Utilisateur à enregistrer.
            {
                Email = request.Email,
                Identifiant = request.Identifiant,
                Password = hashedPassword,
                Role_id = request.Role_id
            };

            var newUtilisateur = await _saveursService.AddUtilisateurAsync(utilisateur);
            // Appelle la couche service pour créer le nouvel utilisateur en base.

            if (newUtilisateur is null)
                return BadRequest("Invalid utilisateur data.");
            // Si la création a échoué, retourne 400 avec un message explicite.

            UtilisateurDTO response = new()
            // Prépare le DTO de sortie renvoyé au client.
            {
                Id = newUtilisateur.Id,
                Identifiant = newUtilisateur.Identifiant,
                Email = newUtilisateur.Email,
                Password = newUtilisateur.Password,
                Role_id = newUtilisateur.Role_id
            };

            return CreatedAtAction(nameof(GetUtilisateurById), new { id = response.Id }, response);
            // Retourne 201, avec l’URL de récupération de la ressource (GetUtilisateurById) et le corps du DTO.
        }

        private object GetUtilisateurById()
        // Méthode privée sans implémentation réelle, qui semble être un doublon/erreur.
        // Elle n’est pas utilisée pour le routing (l’action publique au-dessus l’est déjà).
        {
            throw new NotImplementedException();
            // Lance systématiquement une exception si appelée.
        }

        [HttpPut("{id}")]
        // Action HTTP PUT sur "api/utilisateurs/{id}" pour mettre à jour un utilisateur existant.

        [ProducesResponseType(StatusCodes.Status200OK)]
        // Peut retourner 200 avec l’utilisateur modifié.

        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        // Peut retourner 400 si les données sont invalides.
        public async Task<IActionResult> UpdateUtilisateur(
            IValidator<UpdateUtilisateurDTO> validator,
            [FromRoute] int id,
            [FromBody] UpdateUtilisateurDTO request)
        // Met à jour un utilisateur à partir des données fournies.
        {
            validator.ValidateAndThrow(request);
            // Valide le DTO de mise à jour.

            Utilisateur utilisateur = new()
            // Construit un objet métier Utilisateur à partir du DTO et de l’id de la route.
            {
                Id = id,
                Identifiant = request.Identifiant,
                Email = request.Email,
                Password = request.Password,
                Role_id = request.Role_id
            };

            var modifiedUtilisateur = await _saveursService.ModifyUtilisateurAsync(utilisateur);
            // Demande à la couche service de mettre à jour l’utilisateur.

            if (modifiedUtilisateur is null)
                return BadRequest("Invalid utilisateur.");
            // Si la mise à jour a échoué (ex. utilisateur inexistant), renvoie 400.

            UtilisateurDTO response = new()
            // Prépare le DTO de sortie avec les données mises à jour.
            {
                Id = modifiedUtilisateur.Id,
                Identifiant = modifiedUtilisateur.Identifiant,
                Email = modifiedUtilisateur.Email,
                Role_id = modifiedUtilisateur.Role_id
                // Le mot de passe n’est pas renvoyé ici.
            };

            return Ok(response);
            // Retourne 200 avec l’utilisateur mis à jour.
        }

        [HttpDelete("{id}")]
        // Action HTTP DELETE sur "api/utilisateurs/{id}" pour supprimer un utilisateur.

        [ProducesResponseType(StatusCodes.Status204NoContent)]
        // Peut retourner 204 en cas de suppression réussie (aucun contenu dans la réponse).

        [ProducesResponseType(StatusCodes.Status404NotFound)]
        // Peut retourner 404 si l’utilisateur n’existe pas.
        public async Task<IActionResult> DeleteUtilisateur([FromRoute] int id)
        // Supprime un utilisateur via son identifiant.
        {
            var success = await _saveursService.DeleteUtilisateurAsync(id);
            // Demande à la couche service de supprimer l’utilisateur.

            return success ? NoContent() : NotFound();
            // Si la suppression a réussi, retourne 204 ; sinon 404.
        }
    }
}
