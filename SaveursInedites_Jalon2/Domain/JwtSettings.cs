using FluentValidation;
// Importe la bibliothèque FluentValidation permettant de définir des règles de validation déclaratives.

namespace SaveursInedites_Jalon2.Domain
// Namespace regroupant les classes de configuration et les objets de domaine.
{
    public class JwtSettings : IJwtSettings
    // Classe de configuration contenant les paramètres nécessaires à la génération de tokens JWT.
    // Implémente l’interface IJwtSettings.
    {
        public string Secret { get; set; }
        // Clé secrète utilisée pour signer les tokens JWT.

        public string Issuer { get; set; }
        // Identifie l’émetteur du token (API).

        public string Audience { get; set; }
        // Identifie le ou les clients autorisés à consommer le token.

        public int ExpirationMinutes { get; set; }
        // Durée de validité du token en minutes.
    }

    public class JwtSettingsValidator : AbstractValidator<JwtSettings>
    // Classe de validation FluentValidation pour la configuration JWT.
    // Vérifie la cohérence et la validité des propriétés de JwtSettings.
    {
        public JwtSettingsValidator()
        // Constructeur qui définit les règles de validation.
        {
            const int MinSecretLength = 32;
            // Longueur minimale imposée pour la clé secrète JWT.

            RuleFor(x => x.Secret)
                .NotNull().NotEmpty().WithMessage("Le secret JWT est requis.")
                // Le secret ne doit jamais être null ou vide.

                .MinimumLength(MinSecretLength)
                .WithMessage($"Le secret JWT doit contenir au moins {MinSecretLength} caractères.");
            // Le secret doit être suffisamment long pour garantir la sécurité.

            RuleFor(x => x.Issuer)
                .NotNull().NotEmpty().WithMessage("L'émetteur (Issuer) est requis.");
            // L’émetteur doit obligatoirement être défini.

            RuleFor(x => x.Audience)
                .NotNull().NotEmpty().WithMessage("L'audience (Audience) est requise.");
            // L’audience doit obligatoirement être fournie.

            RuleFor(x => x.ExpirationMinutes)
                .GreaterThan(0).WithMessage("La durée d'expiration doit être supérieure à 0.");
            // Le token doit avoir une durée d’expiration strictement positive.
        }
    }
}
