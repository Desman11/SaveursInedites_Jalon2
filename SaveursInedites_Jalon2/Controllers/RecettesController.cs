using FluentValidation;
// Utilisé pour valider les DTO en entrée.

using Microsoft.AspNetCore.Authorization;
// Fournit les attributs d’autorisation ([Authorize], Roles, etc.).

using Microsoft.AspNetCore.Mvc;
// Fournit les types nécessaires aux contrôleurs Web API.

using SaveursInedites_Jalon2.Domain.BO;
// Accès aux objets métier, ici Recette.

using SaveursInedites_Jalon2.Domain.DTO.DTOIn;
// Accès aux DTO d’entrée (CreateRecetteDTO, UpdateRecetteDTO).

using SaveursInedites_Jalon2.Domain.DTO.DTOOut;
// Accès aux DTO de sortie (RecetteDTO).

using SaveursInedites_Jalon2.Services;
// Accès à ISaveursService, la couche de service métier.

namespace SaveursInedites_Jalon2.Controllers
// Namespace regroupant les contrôleurs de l’API.
{
    /// <summary>
    /// API Recettes : lecture (authentifiés), création/modification/suppression (administrateurs).
    /// </summary>
    [ApiController]
    // Indique qu’il s’agit d’un contrôleur d’API (gestion modèle, binding, erreurs automatiques, etc.).

    [Route("api/[controller]")]
    // Route de base : "api/recettes" (nom du contrôleur sans "Controller").

    [Produces("application/json", "application/problem+json")]
    // Spécifie les formats de réponse retournés : JSON normal ou ProblemDetails.

    [Authorize] // par défaut: authentifié (User ou Admin)
    // Toutes les actions nécessitent un utilisateur authentifié (quel que soit le rôle).
    public class RecettesController : ControllerBase
    // Contrôleur Web API pour la ressource Recette.
    {
        private readonly ISaveursService _recetteService;
        // Service métier injecté pour manipuler les recettes.

        public RecettesController(ISaveursService recetteService)
        // Constructeur recevant le service via l’injection de dépendances.
        {
            _recetteService = recetteService;
            // Affectation au champ privé.
        }

        /// <summary>Liste toutes les recettes.</summary>
        [HttpGet]
        // Action HTTP GET sur "api/recettes".

        [ProducesResponseType(typeof(IEnumerable<RecetteDTO>), StatusCodes.Status200OK)]
        // Indique que l’action peut retourner 200 avec une liste de RecetteDTO.

        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        // Indique qu’elle peut aussi retourner 401 avec un ProblemDetails.
        public async Task<IActionResult> GetRecettes()
        // Récupère et renvoie la liste de toutes les recettes.
        {
            var recettes = await _recetteService.GetAllRecettesAsync();
            // Appel à la couche service pour récupérer toutes les recettes métier.

            var response = recettes.Select(r => new RecetteDTO
            // Projection des objets métier Recette vers des DTO RecetteDTO.
            {
                Id = r.Id,
                Nom = r.Nom,
                TempsPreparation = r.TempsPreparation,
                TempsCuisson = r.TempsCuisson,
                Difficulte = r.Difficulte,
                Photo = r.Photo,
                Createur = r.Createur
                // Le champ Role du DTO n’est pas renseigné ici.
            });

            return Ok(response);
            // Retourne 200 avec la liste sérialisée en JSON.
        }

        /// <summary>Récupère une recette par id.</summary>
        [HttpGet("{id:int}")]
        // Action HTTP GET sur "api/recettes/{id:int}".

        [ProducesResponseType(typeof(RecetteDTO), StatusCodes.Status200OK)]
        // 200 avec une RecetteDTO en cas de succès.

        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        // 401 si non authentifié.

        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        // 404 si la recette n’existe pas.
        public async Task<IActionResult> GetRecetteById([FromRoute] int id)
        // Récupère une recette selon son identifiant.
        {
            var r = await _recetteService.GetRecetteByIdAsync(id);
            // Appel au service pour obtenir la recette.

            if (r is null)
                return Problem(statusCode: 404, title: "Introuvable", detail: $"Recette {id} inexistante.");
            // Si aucune recette trouvée, renvoie un ProblemDetails avec statut 404.

            var response = new RecetteDTO
            // Construction du DTO de sortie à partir de la recette.
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
            // Retourne 200 avec la recette sérialisée.
        }

        /// <summary>Crée une nouvelle recette. (Admin)</summary>
        [HttpPost]
        // Action HTTP POST sur "api/recettes".

        [Authorize(Roles = "Administrateur")]
        // Restreint cette action au rôle Administrateur uniquement.

        [ProducesResponseType(typeof(RecetteDTO), StatusCodes.Status201Created)]
        // 201 avec la recette créée.

        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        // 400 en cas de requête invalide.

        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        // 401 si non authentifié.

        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
        // 403 si authentifié mais sans le rôle Administrateur.
        public async Task<IActionResult> CreateRecette(
            [FromServices] IValidator<CreateRecetteDTO> validator,
            // Récupère le validateur pour CreateRecetteDTO dans le conteneur DI.

            [FromBody] CreateRecetteDTO request)
        // DTO reçu dans le corps de la requête POST.
        {
            validator.ValidateAndThrow(request);
            // Valide le DTO; lève une ValidationException si les règles échouent.

            // L'Id est géré par la base; on ignore request.Id s'il existe
            var entity = new Recette
            // Création d’une entité Recette à partir du DTO (hors Id).
            {
                Nom = request.Nom,
                TempsPreparation = request.TempsPreparation,
                TempsCuisson = request.TempsCuisson,
                Difficulte = request.Difficulte,
                Photo = request.Photo,
                Createur = request.Createur
            };

            var created = await _recetteService.AddRecetteAsync(entity);
            // Appel à la couche service pour persister la recette.

            if (created is null)
                return Problem(statusCode: 400, title: "Requête invalide", detail: "Création impossible.");
            // Si la création échoue, retourne un ProblemDetails 400.

            var response = new RecetteDTO
            // Convertit l’entité créée en DTO de sortie.
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
            // Retourne 201 Created, avec l’URL de récupération (GetRecetteById) et le DTO dans le corps.
        }

        /// <summary>Met à jour une recette. (Admin)</summary>
        [HttpPut("{id:int}")]
        // Action HTTP PUT sur "api/recettes/{id:int}".

        [Authorize(Roles = "Administrateur")]
        // Action réservée au rôle Administrateur.

        [ProducesResponseType(typeof(RecetteDTO), StatusCodes.Status200OK)]
        // 200 avec la recette mise à jour.

        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        // 400 si la requête est invalide.

        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        // 401 si non authentifié.

        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
        // 403 si pas le bon rôle.

        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        // 404 si la recette à mettre à jour n’existe pas.
        public async Task<IActionResult> UpdateRecette(
            [FromServices] IValidator<UpdateRecetteDTO> validator,
            // Validateur pour le DTO de mise à jour.

            [FromRoute] int id,
            // Id de la recette à modifier (source de vérité).

            [FromBody] UpdateRecetteDTO request)
        // DTO contenant les nouvelles valeurs de la recette.
        {
            validator.ValidateAndThrow(request);
            // Valide le DTO de mise à jour.

            var existing = await _recetteService.GetRecetteByIdAsync(id);
            // Vérifie d’abord si la recette existe en base.

            if (existing is null)
                return Problem(statusCode: 404, title: "Introuvable", detail: $"Recette {id} inexistante.");
            // Si elle n’existe pas, renvoie 404.

            var toUpdate = new Recette
            // Construit une nouvelle entité Recette avec les données mises à jour.
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
            // Appel à la couche service pour persister la mise à jour.

            if (updated is null)
                return Problem(statusCode: 400, title: "Requête invalide", detail: "Mise à jour impossible.");
            // Si la mise à jour échoue, renvoie 400.

            var response = new RecetteDTO
            // Prépare le DTO de sortie avec les valeurs mises à jour.
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
            // Retourne 200 avec la recette mise à jour.
        }

        /// <summary>Supprime une recette. (Admin)</summary>
        [HttpDelete("{id:int}")]
        // Action HTTP DELETE sur "api/recettes/{id:int}".

        [Authorize(Roles = "Administrateur")]
        // Réservée aux administrateurs.

        [ProducesResponseType(StatusCodes.Status204NoContent)]
        // 204 si suppression réussie.

        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        // 401 en cas de non-authentification.

        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
        // 403 si le rôle n’est pas Administrateur.

        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        // 404 si la recette n’existe pas.
        public async Task<IActionResult> DeleteRecette([FromRoute] int id)
        // Supprime la recette correspondant à l’id fourni.
        {
            var ok = await _recetteService.DeleteRecetteAsync(id);
            // Appel à la couche service pour supprimer la recette.

            if (!ok)
                return Problem(statusCode: 404, title: "Introuvable", detail: $"Recette {id} inexistante.");
            // Si la suppression n’a pas réussi (par ex. recette introuvable), renvoie 404.

            return NoContent();
            // Retourne 204 No Content en cas de succès.
        }
    }
}
