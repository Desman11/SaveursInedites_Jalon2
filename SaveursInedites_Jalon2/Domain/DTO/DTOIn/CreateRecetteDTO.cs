using FluentValidation;
// Import de FluentValidation pour définir des règles de validation sur les données reçues.

namespace SaveursInedites_Jalon2.Domain.DTO.DTOIn
// Namespace contenant les DTO envoyés par le client vers l’API (entrées).
{
    public class CreateRecetteDTO
    // DTO utilisé pour créer une nouvelle recette.
    {
        public int Id { get; set; }
        // Identifiant. Souvent non requis lors de la création, mais présent ici pour cohérence.

        public string Nom { get; set; } = string.Empty;
        // Nom de la recette. Chaîne vide par défaut pour éviter les null.

        public TimeSpan TempsPreparation { get; set; }
        // Durée de préparation.

        public TimeSpan TempsCuisson { get; set; } = TimeSpan.Zero;
        // Durée de cuisson. Par défaut : aucune cuisson.

        public int Difficulte { get; set; }
        // Niveau de difficulté (généralement une valeur entre 1 et 5 selon l'application).

        public string? Photo { get; set; }
        // URL, chemin ou nom de fichier pour l’image. Optionnel.

        public int Createur { get; set; }
        // Identifiant de l’utilisateur ayant créé la recette.
    }

    public class CreateRecetteDTOValidator : AbstractValidator<CreateRecetteDTO>
    // Validateur FluentValidation pour contrôler les données lors de la création d’une recette.
    {
        public CreateRecetteDTOValidator()
        // Constructeur dans lequel les règles de validation sont définies.
        {
            // Exemple de configuration (commentée) permettant d’arrêter immédiatement en cas d’erreur :
            // RuleLevelCascadeMode = CascadeMode.Stop;
            // ClassLevelCascadeMode = CascadeMode.Stop;

            RuleFor(r => r.Nom)
                .NotNull()
                .NotEmpty()
                .WithMessage("Le nom est obligatoire.");
            // Le nom doit impérativement être renseigné.

            RuleFor(r => r.TempsPreparation)
                .NotNull()
                .NotEmpty()
                .WithMessage("Le Temps de préparation est obligatoire.");
            // Le temps de préparation doit être fourni.
        }
    }
}
