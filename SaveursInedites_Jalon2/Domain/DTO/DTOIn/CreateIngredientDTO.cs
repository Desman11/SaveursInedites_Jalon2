using FluentValidation;
// Import de FluentValidation pour appliquer des règles de validation aux DTO.

namespace SaveursInedites_Jalon2.Domain.DTO.DTOIn
// Namespace contenant les DTO reçus par l’API (données en entrée).
{
    public class CreateIngredientDTO
    // DTO utilisé lors de la création d’un nouvel ingrédient.
    {
        public int Id { get; set; }
        // Identifiant de l’ingrédient (souvent non nécessaire en phase de création mais présent ici).

        public string Nom { get; set; } = string.Empty;
        // Nom de l’ingrédient. Chaîne vide par défaut pour éviter une valeur null.
    }

    public class CreateIngredientDTOValidator : AbstractValidator<CreateIngredientDTO>
    // Validateur FluentValidation chargé de vérifier la validité des données envoyées pour créer un ingrédient.
    {
        public CreateIngredientDTOValidator()
        // Constructeur définissant les règles de validation pour CreateIngredientDTO.
        {
            // Exemples de modes de cascade (commentés) pour arrêter la validation dès la première erreur :
            // RuleLevelCascadeMode = CascadeMode.Stop;
            // ClassLevelCascadeMode = CascadeMode.Stop;

            RuleFor(r => r.Nom)
                .NotNull()
                .NotEmpty()
                .WithMessage("Le nom est obligatoire.");
            // L’ingrédient doit impérativement avoir un nom.
        }
    }
}
