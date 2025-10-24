using FluentValidation;

namespace SaveursInedites_Jalon2.Domain.DTO.DTOIn
{
    public class CreateUtilisateurDTO
    {
        public string Identifiant { get; set; } = string.Empty;

     
        public string Email { get; set; } = string.Empty;

     
        public string Password { get; set; } = string.Empty;

        public string Role { get; set; } = "User";
    }
    public class CreateUtilisateursDTOValidator : AbstractValidator<CreateUtilisateurDTO>
    {
        public CreateUtilisateursDTOValidator()
        {
            // Arrêter la validation dès qu'une règle échoue
            //RuleLevelCascadeMode = CascadeMode.Stop;
            //ClassLevelCascadeMode = CascadeMode.Stop;

            RuleFor(r => r.Identifiant).NotNull().NotEmpty().WithMessage("L'identifiant est obligatoire.");
            RuleFor(r => r.Email).NotNull().NotEmpty().WithMessage("L'email est obligatoire.");
            RuleFor(r => r.Password).NotNull().NotEmpty().WithMessage("Le mot de passe est obligatoire.");
        }
    }
}


