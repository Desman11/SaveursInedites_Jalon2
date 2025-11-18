using FluentValidation;
// Import de FluentValidation pour définir les règles de validation.

namespace SaveursInedites_Jalon2.Domain.DTO.DTOIn
// Namespace regroupant les DTO envoyés par le client (entrées API).
{
    public class CreateUtilisateurDTO
    // DTO utilisé pour créer un nouvel utilisateur.
    {
        public string Identifiant { get; set; } = string.Empty;
        // Nom d’utilisateur (login) à créer. Chaîne vide par défaut pour éviter les valeurs null.

        public string Email { get; set; } = string.Empty;
        // Adresse email associée au nouvel utilisateur.

        public string Password { get; set; } = string.Empty;
        // Mot de passe initial du nouvel utilisateur.

        public int Role_id { get; set; }
        // Identifiant du rôle (ex. utilisateur, administrateur).
    }

    public class CreateUtilisateursDTOValidator : AbstractValidator<CreateUtilisateurDTO>
    // Validateur FluentValidation chargé de contrôler la validité des données lors de la création d'un utilisateur.
    {
        public CreateUtilisateursDTOValidator()
        // Constructeur dans lequel les règles de validation sont définies.
        {
            RuleFor(r => r.Identifiant)
                .NotNull()
                .NotEmpty()
                .WithMessage("L'identifiant est obligatoire.");
            // L’identifiant ne doit pas être null ou vide.

            RuleFor(r => r.Email)
                .NotNull()
                .NotEmpty()
                .WithMessage("L'email est obligatoire.");
            // L’email doit être renseigné.

            RuleFor(r => r.Password)
                .NotNull()
                .NotEmpty()
                .WithMessage("Le mot de passe est obligatoire.");
            // Le mot de passe est obligatoire pour créer un utilisateur.

            RuleFor(r => r.Role_id)
                .GreaterThan(0)
                .WithMessage("Le rôle est obligatoire et doit être supérieur à 0.");
            // Le rôle doit être défini et supérieur à zéro (0 = non valide).
        }
    }
}
