namespace SaveursInedites_Jalon2.Domain
// Espace de noms contenant les objets de configuration de l’application.
{
    public interface IDatabaseSettings
    // Interface définissant les paramètres nécessaires pour configurer l’accès à la base de données.
    {
        string ConnectionString { get; set; }
        // Chaîne de connexion permettant d’accéder à la base de données sélectionnée.

        DatabaseProviderName? DatabaseProviderName { get; set; }
        // Indique le fournisseur de base de données utilisé (PostgreSQL, MySQL, etc.).
        // Nullable pour autoriser l’absence de valeur explicite si nécessaire.
    }
}
