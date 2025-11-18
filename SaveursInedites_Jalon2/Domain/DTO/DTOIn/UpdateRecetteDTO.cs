using FluentValidation;
// Import de la bibliothèque FluentValidation pour définir les règles de validation.

namespace SaveursInedites_Jalon2.Domain.DTO.DTOIn
// Namespace contenant les DTO envoyés par le client à l’API (données en entrée).
{
    public class UpdateRecetteDTO
    // DTO utilisé pour mettre à jour une recette existante.
    {
        public int Id { get; set; }
        // Identifiant unique de la recette à mettre à jour.

        public string Nom { get; set; } = string.Empty;
        // Nom de la recette. Par défaut, une chaîne vide pour éviter un null.

        public TimeSpan TempsPreparation { get; set; }
        // Durée de préparation.

        public TimeSpan TempsCuisson { get; set; } = TimeSpan.Zero;
        // Durée de cuisson. Valeur par défaut : 0.

        public int Difficulte { get; set; }
        // Niveau de difficulté (valeur entière).

        public string? Photo { get; set; }
        // Chemin, nom de fichier ou URL représentant la photo de la recette.
        // Nullable car non obligatoire.

        public int Createur { get; set; }
        // Identifiant de l’utilisateur ayant créé la recette.
    }

    public class UpdateRecetteDTOValidator : AbstractValidator<UpdateRecetteDTO>
    // Validateur FluentValidation associé au DTO de mise à jour d’une recette.
    {
        public UpdateRecetteDTOValidator()
        // Constructeur dans lequel sont définies les règles de validation.
        {
            // Exemple de configuration permettant d'arrêter au premier échec (commentée ici) :
            // RuleLevelCascadeMode = CascadeMode.Stop;
            // ClassLevelCascadeMode = CascadeMode.Stop;

            RuleFor(r => r.Nom)
                .NotNull().NotEmpty().WithMessage("Le nom est obligatoire.");
            // La recette doit obligatoirement avoir un nom.

            RuleFor(r => r.TempsPreparation)
                .NotNull().NotEmpty().WithMessage("Le Temps de préparation est obligatoire.");
            // Un temps de préparation doit être fourni.
        }
    }
}
