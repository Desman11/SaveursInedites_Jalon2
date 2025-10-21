using FluentValidation;

namespace SaveursInedites_Jalon2.Domain.DTO.In
{
    /// <summary>
    /// DTO utilisé pour la mise à jour d'un ingredient.
    /// </summary>
    public class UpdateIngredientDTO
    {

        /// <summary>
        /// Nom de l'ingredient.
        /// </summary>
        public string Nom { get; set; }

   
    }

   

    /// <summary>
    /// Validateur FluentValidation pour <see cref="UpdateIngredientDTO"/>.
    /// </summary>
    public class UpdateIngredientsDTOValidator : AbstractValidator<UpdateIngredientDTO>
    {
        /// <summary>
        /// Initialise les règles de validation pour la mise à jour d'un ingredient.
        /// </summary>
        public UpdateIngredientsDTOValidator()
        {
            RuleFor(i => i.Nom).NotNull().NotEmpty().WithMessage("Le nom de l'ingredient est obligatoire.");
        }
    }
}


