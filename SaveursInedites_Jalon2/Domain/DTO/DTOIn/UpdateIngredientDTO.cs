using FluentValidation;
// Import de FluentValidation pour définir les règles de validation.

namespace SaveursInedites_Jalon2.Domain.DTO.DTOIn
// Namespace regroupant les DTO utilisés en entrée (requêtes clients).
{
    /// <summary>
    /// DTO utilisé pour la mise à jour d'un ingredient.
    /// </summary>
    public class UpdateIngredientDTO
    // Modèle représentant les données nécessaires pour modifier un ingrédient existant.
    {
        public int Id { get; set; }
        // Identifiant de l’ingrédient à mettre à jour.

        /// <summary>
        /// Nom de l'ingredient.
        /// </summary>
        public string Nom { get; set; }
        // Nouveau nom de l’ingrédient.
    }

    /// <summary>
    /// Validateur FluentValidation pour <see cref="UpdateIngredientDTO"/>.
    /// </summary>
    public class UpdateIngredientsDTOValidator : AbstractValidator<UpdateIngredientDTO>
    // Classe définissant les règles de validation appliquées lors de la mise à jour d’un ingrédient.
    {
        /// <summary>
        /// Initialise les règles de validation pour la mise à jour d'un ingredient.
        /// </summary>
        public UpdateIngredientsDTOValidator()
        // Constructeur où les règles FluentValidation sont définies.
        {
            RuleFor(i => i.Nom)
                .NotNull().NotEmpty().WithMessage("Le nom de l'ingredient est obligatoire.");
            // Le nom doit obligatoirement être renseigné.
        }
    }
}
