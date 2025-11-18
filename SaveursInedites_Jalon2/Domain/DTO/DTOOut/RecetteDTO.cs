namespace SaveursInedites_Jalon2.Domain.DTO.DTOOut
// Namespace regroupant les DTO destinés à être envoyés au client (DTO de sortie).
{
    public class RecetteDTO
    // Représentation d’une recette exposée par l’API vers un client (web, WinForms, etc.).
    {
        public int Id { get; set; }
        // Identifiant unique de la recette.

        public string Nom { get; set; }
        // Nom ou titre de la recette.

        public TimeSpan TempsPreparation { get; set; }
        // Durée nécessaire pour préparer la recette.

        public TimeSpan TempsCuisson { get; set; }
        // Durée de cuisson de la recette.

        public int Difficulte { get; set; }
        // Niveau de difficulté (ex. 1 = facile, 2 = moyen, 3 = difficile...).

        public string? Photo { get; set; }
        // Chemin, nom de fichier ou URL de l’éventuelle photo associée à la recette.
        // Nullable car une recette peut ne pas avoir d’image.

        public int Createur { get; set; }
        // Identifiant de l’utilisateur ayant créé la recette.

        public int Role { get; set; }
        // Rôle associé au créateur (user, admin...). Peut servir pour filtrer ou afficher des droits.
    }
}
