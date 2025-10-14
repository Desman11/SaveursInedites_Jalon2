namespace SaveursInedites_Jalon2.Domain.BO
{
     public class Recette
    {
        // Identifiant unique (SERIAL en base)
        public int Id { get; set; }

        // Nom de la recette (VARCHAR(100) NOT NULL)
        public string Nom { get; set; } = string.Empty;

        // Temps de préparation (INTERVAL en PostgreSQL)
        public TimeSpan TempsPreparation { get; set; }

        // Temps de cuisson (INTERVAL avec DEFAULT '00:00:00')
        public TimeSpan TempsCuisson { get; set; } = TimeSpan.Zero;

        // Difficulté (INT entre 1 et 5)
        public int Difficulte { get; set; }

        // Photo (VARCHAR(100), peut être NULL)
        public string? Photo { get; set; }

        // Clé étrangère vers la table utilisateurs
        public int Createur { get; set; }

     
    }
}
