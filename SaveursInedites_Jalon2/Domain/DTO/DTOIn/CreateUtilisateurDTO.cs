using FluentValidation;

namespace SaveursInedites_Jalon2.Domain.DTO.DTOIn
{
    public class CreateUtilisateurDTO
    {
        public string Identifiant { get; set; } = string.Empty;

     
        public string Email { get; set; } = string.Empty;

     
        public string Password { get; set; } = string.Empty;

        public int Role_id { get; set; }
    }
    public class CreateUtilisateursDTOValidator : AbstractValidator<CreateUtilisateurDTO>
    {
        public CreateUtilisateursDTOValidator()
        {
            RuleFor(r => r.Identifiant)
                .NotNull()
                .NotEmpty()
                .WithMessage("L'identifiant est obligatoire.");

            RuleFor(r => r.Email)
                .NotNull()
                .NotEmpty()
                .WithMessage("L'email est obligatoire.");

            RuleFor(r => r.Password)
                .NotNull()
                .NotEmpty()
                .WithMessage("Le mot de passe est obligatoire.");

            RuleFor(r => r.Role_id)
                .GreaterThan(0)
                .WithMessage("Le rôle est obligatoire et doit être supérieur à 0.");
        }
    }
    }


