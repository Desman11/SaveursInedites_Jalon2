using Google.Protobuf.WellKnownTypes;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
// Espaces de noms importés ici, mais non utilisés directement dans cette classe.
// Ils peuvent être des restes d'anciens usages ou générés automatiquement.

namespace SaveursInedites_Jalon2.Domain.DTO
// Namespace dans lequel se trouve ce middleware et la classe de réponse d’erreur.
{
    public class GlobalExceptionMiddleware
    // Middleware global chargé d’intercepter les exceptions non gérées
    // et de renvoyer une réponse JSON standardisée.
    {
        private readonly RequestDelegate _next;
        // Représente le middleware suivant dans le pipeline HTTP.

        private readonly ILogger<GlobalExceptionMiddleware> _logger;
        // Logger typé permettant de journaliser les exceptions interceptées.

        private readonly IWebHostEnvironment _env;
        // Fournit des informations sur l’environnement d’exécution (Development, Production, etc.).

        public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger, IWebHostEnvironment env)
        // Constructeur recevant les dépendances via l’injection de dépendances.
        {
            _next = next;
            // Stocke le middleware suivant.

            _logger = logger;
            // Stocke le logger injecté.

            _env = env;
            // Stocke l’environnement d’exécution.
        }

        public async Task InvokeAsync(HttpContext context)
        // Méthode appelée pour chaque requête HTTP traversant ce middleware.
        {
            try
            {
                await _next(context);
                // Appelle le middleware suivant dans le pipeline.
            }
            catch (Exception ex)
            {
                // En cas d’exception non gérée dans les middlewares suivants, on arrive ici.
                _logger.LogError(ex, "\r\nException interceptée globalement.\r\n");
                // Journalise l’exception avec un message contextualisé.

                await HandleExceptionAsync(context, ex);
                // Construit et renvoie une réponse HTTP appropriée au client.
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        // Méthode qui prépare une réponse JSON en fonction du type d’exception.
        {
            // Selon le type d’exception, on peut choisir différents codes HTTP et messages.
            context.Response.ContentType = "application/json";
            // Force le type de contenu de la réponse en JSON.

            if (exception is FluentValidation.ValidationException fvex)
            // Si l’exception est une erreur de validation FluentValidation...
            {
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                // ... on renvoie un code 400 (Bad Request).

                ErrorResponse response = new()
                {
                    Error = "Des erreurs de validation sont survenues.",
                    // Message général d’erreur.

                    Details = string.Join("\r", fvex.Errors.Select(e => e.ErrorMessage))
                    // Concatène tous les messages d’erreur de validation, séparés par des retours chariot.
                };
                return context.Response.WriteAsJsonAsync(response);
                // Sérialise l’objet ErrorResponse en JSON et l’écrit dans la réponse HTTP.
            }
            else
            // Pour tous les autres types d’exception...
            {
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                // ... on renvoie un code 500 (erreur interne serveur).

                ErrorResponse response = new()
                {
                    Error = "Une erreur interne est survenue.",
                    // Message générique pour une erreur serveur.

                    Details = _env.IsDevelopment()
                        ? $"{exception.GetType().Name} : {exception.Message}"
                        // En environnement de développement, on expose le type de l’exception et son message.

                        : "Veuillez vous adresser à l'administrateur du système."
                    // En production, on masque le détail technique pour des raisons de sécurité.
                };
                return context.Response.WriteAsJsonAsync(response);
                // Écrit la réponse d’erreur au format JSON.
            }
        }

        class ErrorResponse
        // Classe interne représentant la structure JSON renvoyée en cas d’erreur.
        {
            public string Error { get; set; }
            // Message d’erreur principal.

            public string Details { get; set; }
            // Détails supplémentaires sur l’erreur.
        }
    }
}
