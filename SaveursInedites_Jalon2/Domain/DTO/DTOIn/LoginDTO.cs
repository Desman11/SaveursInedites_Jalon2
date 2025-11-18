using FluentValidation;
// Import de FluentValidation pour définir des règles de validation sur les DTO.

namespace SaveursInedites_Jalon2.Domain.DTO.DTOIn
// Namespace regroupant les DTO reçus par l’API.
{
    /// <summary>
    /// DTO utilisé pour la connexion d'un utilisateur.
    /// </summary>
    public class LoginDTO
    // Modèle contenant les informations nécessaires pour authentifier un utilisateur.
    {
        /// <summary>
        /// Nom d'utilisateur de l'utilisateur.
        /// </summary>
        public string Username { get; set; }
        // Identifiant de connexion fourni par l’utilisateur.

        /// <summary>
        /// Mot de passe de l'utilisateur.
        /// </summary>
        public string Password { get; set; }
        // Mot de passe fourni pour la connexion.
    }

    /// <summary>
    /// Validateur FluentValidation pour <see cref="LoginDTO"/>.
    /// </summary>
    public class LoginDTOValidator : AbstractValidator<LoginDTO>
    // Classe définissant les règles de validation appliquées au login.
    {
        /// <summary>
        /// Initialise les règles de validation pour la connexion d'un utilisateur.
        /// </summary>
        public LoginDTOValidator()
        // Constructeur où sont définies les règles aptes à sécuriser la saisie utilisateur.
        {
            RuleFor(x => x.Username)
                .NotEmpty().WithMessage("Le nom d'utilisateur est requis.")
                // Le nom d’utilisateur doit obligatoirement être renseigné.

                .MinimumLength(3).WithMessage("Le nom d'utilisateur doit contenir au moins 3 caractères.");
            // Le nom d’utilisateur doit comporter un minimum de 3 caractères.

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Le mot de passe est requis.")
                // Le mot de passe est obligatoire.

                .MinimumLength(4).WithMessage("Le mot de passe doit contenir au moins 4 caractères.");
            // Longueur minimale imposée pour plus de sécurité.
        }
    }
}
