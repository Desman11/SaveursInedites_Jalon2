using Google.Protobuf.WellKnownTypes;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

// Imports non utilisés directement dans ce fichier, probablement présents
// car le projet utilise ces namespaces ailleurs.

namespace SaveursInedites_Jalon2.Domain
// Namespace regroupant les composants transversaux du domaine, dont ce middleware.
{
    public class GlobalExceptionMiddleware
    // Middleware personnalisé permettant d'intercepter globalement toutes les exceptions
    // et de renvoyer une réponse JSON unifiée.
    {
        private readonly RequestDelegate _next;
        // Représente le middleware suivant dans le pipeline.

        private readonly ILogger<GlobalExceptionMiddleware> _logger;
        // Permet de journaliser les erreurs interceptées.

        private readonly IWebHostEnvironment _env;
        // Permet de savoir si l'application tourne en mode Développement ou Production.

        public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger, IWebHostEnvironment env)
        // Constructeur recevant les dépendances nécessaires via l’injection de dépendances.
        {
            _next = next;
            _logger = logger;
            _env = env;
        }

        public async Task InvokeAsync(HttpContext context)
        // Méthode principale appelée pour chaque requête HTTP.
        // Exécute le middleware suivant et intercepte les erreurs éventuelles.
        {
            try
            {
                await _next(context);
                // Exécution du reste du pipeline HTTP.

                // Ici, 401 et 403 ne lèvent pas d'exception -> on les intercepte après coup.
                if (context.Response.StatusCode == StatusCodes.Status401Unauthorized)
                {
                    await HandleStatusCodeAsync(context, 401);
                }
                else if (context.Response.StatusCode == StatusCodes.Status403Forbidden)
                {
                    await HandleStatusCodeAsync(context, 403);
                }
            }
            catch (Exception ex)
            {
                // Log de l'exception interceptée.
                _logger.LogError(ex, "\r\nException interceptée globalement.\r\n");

                // Production d'une réponse JSON adaptée.
                await HandleExceptionAsync(context, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        // Méthode qui construit une réponse JSON en fonction du type d'exception interceptée.
        {
            context.Response.ContentType = "application/json";

            // Cas : erreurs de validation FluentValidation.
            if (exception is FluentValidation.ValidationException fvex)
            {
                context.Response.StatusCode = StatusCodes.Status400BadRequest;

                ErrorResponse response = new()
                {
                    Error = "Des erreurs de validation sont survenues.",
                    Details = string.Join("\r", fvex.Errors.Select(e => e.ErrorMessage))
                };

                return context.Response.WriteAsJsonAsync(response);
            }
            // Cas : accès non autorisé.
            else if (exception is UnauthorizedAccessException)
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;

                ErrorResponse response = new()
                {
                    Error = "Accès non autorisé.",
                    Details = exception.Message
                };

                return context.Response.WriteAsJsonAsync(response);
            }
            // Cas : erreur interne générique.
            else
            {
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;

                ErrorResponse response = new()
                {
                    Error = "Une erreur interne est survenue.",
                    Details = _env.IsDevelopment()
                        ? $"{exception.GetType().Name} : {exception.Message}"
                        : "Veuillez vous adresser à l'administrateur du système."
                };

                return context.Response.WriteAsJsonAsync(response);
            }
        }

        private Task HandleStatusCodeAsync(HttpContext context, int statusCode)
        // Méthode permettant de traiter les statuts HTTP renvoyés sans exception (ex : 401/403).
        {
            context.Response.ContentType = "application/json";

            ErrorResponse response = statusCode switch
            {
                401 => new ErrorResponse
                {
                    Error = "Accès non autorisé.",
                    Details = "Vous devez être authentifié pour accéder à cette ressource."
                },

                403 => new ErrorResponse
                {
                    Error = "Accès interdit.",
                    Details = "Vous n'avez pas les droits nécessaires pour accéder à cette ressource."
                },

                _ => null
            };

            // Si un message a été généré, retourne une réponse JSON.
            return response != null
                ? context.Response.WriteAsJsonAsync(response)
                : Task.CompletedTask;
        }
    }

    public class ErrorResponse
    // Structure simple utilisée pour renvoyer les erreurs en JSON.
    {
        public string Error { get; set; }
        // Message général de l’erreur.

        public string Details { get; set; }
        // Informations supplémentaires sur l’erreur.
    }
}
