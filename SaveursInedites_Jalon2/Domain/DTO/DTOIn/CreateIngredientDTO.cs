using FluentValidation;

namespace SaveursInedites_Jalon2.Domain.DTO.DTOIn
{
    public class CreateIngredientDTO
    {
        public int Id { get; set; }

        public string Nom { get; set; } = string.Empty;


    }
    public class CreateIngredientDTOValidator : AbstractValidator<CreateIngredientDTO>
    {
        public CreateIngredientDTOValidator()
        {
            // Arrêter la validation dès qu'une règle échoue
            //RuleLevelCascadeMode = CascadeMode.Stop;
            //ClassLevelCascadeMode = CascadeMode.Stop;

            RuleFor(r => r.Nom).NotNull().NotEmpty().WithMessage("Le nom est obligatoire.");
        }
    }
}


