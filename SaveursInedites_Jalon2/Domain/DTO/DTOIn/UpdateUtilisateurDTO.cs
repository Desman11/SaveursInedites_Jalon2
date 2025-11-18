using FluentValidation;
// Import de FluentValidation pour définir des règles de validation.

namespace SaveursInedites_Jalon2.Domain.DTO.DTOIn
// Namespace contenant les DTO reçus en entrée par l’API.
{
    /// <summary>
    /// DTO utilisé pour la mise à jour d'un utilisateur.
    /// </summary>
    public class UpdateUtilisateurDTO
    // Représentation des données nécessaires pour modifier un utilisateur.
    {
        public int Id { get; set; }
        // Identifiant de l'utilisateur à mettre à jour.

        public string Identifiant { get; set; }
        // Nom d’utilisateur (login).

        public string Email { get; set; }
        // Adresse email de l’utilisateur.

        public string Password { get; set; }
        // Nouveau mot de passe ou mot de passe mis à jour.

        public int Role_id { get; set; }
        // Identifiant du rôle associé à l'utilisateur.
    }

    /// <summary>
    /// Validateur FluentValidation pour <see cref="UpdateUtilisateurDTO"/>.
    /// </summary>
    public class UpdateUtilisateurDTOValidator : AbstractValidator<UpdateUtilisateurDTO>
    // Définit les règles de validation pour la mise à jour d'un utilisateur.
    {
        /// <summary>
        /// Initialise les règles de validation pour la mise à jour d'un utilisateur.
        /// </summary>
        public UpdateUtilisateurDTOValidator()
        // Constructeur où les règles FluentValidation sont définies.
        {
            RuleFor(b => b.Email)
                .NotNull().NotEmpty().WithMessage("L'Email est obligatoire.");
            // L’email doit être renseigné.

            RuleFor(b => b.Password)
                .NotNull().NotEmpty().WithMessage("Le mot de passe est obligatoire.");
            // Le mot de passe doit être fourni.
        }
    }
}
