using FluentValidation;

namespace SaveursInedites_Jalon2.Domain.DTO.In
{
    public class CreateRecetteDTO
    {
        public int Id { get; set; }

        public string Nom { get; set; } = string.Empty;

        public TimeSpan TempsPreparation { get; set; }

        public TimeSpan TempsCuisson { get; set; } = TimeSpan.Zero;

        public int Difficulte { get; set; }

        public string? Photo { get; set; }

        public int Createur { get; set; }
    }
    public class CreateRecetteDTOValidator : AbstractValidator<CreateRecetteDTO>
    {
        public CreateRecetteDTOValidator()
        {
            // Arrêter la validation dès qu'une règle échoue
            //RuleLevelCascadeMode = CascadeMode.Stop;
            //ClassLevelCascadeMode = CascadeMode.Stop;

            RuleFor(r => r.Nom).NotNull().NotEmpty().WithMessage("Le nom est obligatoire.");
            RuleFor(r => r.TempsPreparation).NotNull().NotEmpty().WithMessage("Le Temps de préparation est obligatoire.");
        }
    }
}


