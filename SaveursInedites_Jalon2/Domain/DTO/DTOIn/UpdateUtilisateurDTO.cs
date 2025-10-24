using FluentValidation;

namespace SaveursInedites_Jalon2.Domain.DTO.DTOIn
{
    /// <summary>
    /// DTO utilisé pour la mise à jour d'un utilisateur.
    /// </summary>
    public class UpdateUtilisateurDTO
    {
       
        public int Id { get; set; }

       
        public string Identifiant { get; set; } 

       
        public string Email { get; set; } 

       
        public string Password { get; set; } 

        public int Role { get; set; }
    }

    /// <summary>
    /// Validateur FluentValidation pour <see cref="UpdateUtilisateurDTO"/>.
    /// </summary>
    public class UpdateUtilisateurDTOValidator : AbstractValidator<UpdateUtilisateurDTO>
    {
        /// <summary>
        /// Initialise les règles de validation pour la mise à jour d'un utilisateur.
        /// </summary>
        public UpdateUtilisateurDTOValidator()
        {
            RuleFor(b => b.Email).NotNull().NotEmpty().WithMessage("L'Email est obligatoire.");
            RuleFor(b => b.Password).NotNull().NotEmpty().WithMessage("Le mot de passe est obligatoire.");
        }
    }
}
