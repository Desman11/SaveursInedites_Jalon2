namespace SaveursInedites_Jalon2.Domain.BO
// Namespace regroupant les objets métiers (Business Objects) du domaine.
{
    public class Recette
    // Classe représentant une recette dans le modèle métier.
    {
        // Identifiant unique (SERIAL en base)
        public int Id { get; set; }
        // Clé primaire de la recette en base de données.

        // Nom de la recette (VARCHAR(100) NOT NULL)
        public string Nom { get; set; } = string.Empty;
        // Intitulé de la recette. Toujours non nul grâce à une valeur par défaut.

        // Temps de préparation (INTERVAL en PostgreSQL)
        public TimeSpan TempsPreparation { get; set; }
        // Durée nécessaire pour préparer la recette.

        // Temps de cuisson (INTERVAL avec DEFAULT '00:00:00')
        public TimeSpan TempsCuisson { get; set; } = TimeSpan.Zero;
        // Durée de cuisson. Par défaut à zéro s’il n’y a pas de cuisson.

        // Difficulté (INT entre 1 et 5)
        public int Difficulte { get; set; }
        // Niveau de difficulté, généralement sur une échelle préétablie (ex. 1 à 5).

        // Photo (VARCHAR(100), peut être NULL)
        public string? Photo { get; set; }
        // Chemin, nom de fichier ou URL de la photo. Champ optionnel.

        // Clé étrangère vers la table utilisateurs
        public int Createur { get; set; }
        // Identifiant de l’utilisateur ayant créé la recette.
    }
}
