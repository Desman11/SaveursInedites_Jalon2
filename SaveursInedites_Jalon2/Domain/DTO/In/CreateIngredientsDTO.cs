using FluentValidation;

namespace SaveursInedites_Jalon2.Domain.DTO.In
{
    /// <summary>
    /// DTO utilisé pour la création d'un ingredient.
    /// </summary>
    public class CreateIngredientsDTO
    {
        /// <summary>
        /// Nom de l'ingredient.
        /// </summary>
        public string Nom { get; set; }

        /// <summary>
        /// Description de l'ingredient.
        /// </summary>
   
    }

    /// <summary>
    /// Validateur FluentValidation pour <see cref="CreateIngredientsDTO"/>.
    /// </summary>
    public class CreateIngredientsDTOValidator : AbstractValidator<CreateIngredientsDTO>
    {
        /// <summary>
        /// Initialise les règles de validation pour la création d'un ingredient.
        /// </summary>
        public CreateIngredientsDTOValidator()
        {
            RuleFor(i => i.Nom).NotNull().NotEmpty().WithMessage("Le nom de l'ingredient est obligatoire.");
            
        }
    }
}

