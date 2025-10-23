using FluentValidation;

namespace SaveursInedites_Jalon2.Domain.DTO.In
{
    /// <summary>
    /// DTO utilisé pour la création d'un ingredient.
    /// </summary>
    public class CreateIngredientDTO
    {
        
        public string Nom { get; set; }
    }

    /// <summary>
    /// Validateur FluentValidation pour <see cref="CreateIngredientDTO"/>.
    /// </summary>
    public class CreateIngredientDTOValidator : AbstractValidator<CreateIngredientDTO>
    {
        /// <summary>
        /// Initialise les règles de validation pour la création d'un auteur.
        /// </summary>
        public CreateIngredientDTOValidator()
        {
            RuleFor(a => a.Nom).NotNull().NotEmpty().WithMessage("Le nom est obligatoire.");
        }
    }
}
