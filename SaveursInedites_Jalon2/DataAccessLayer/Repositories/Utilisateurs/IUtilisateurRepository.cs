using SaveursInedites_Jalon2.Domain.BO;
// Import de la classe métier Utilisateur.

namespace SaveursInedites_Jalon2.DataAccessLayer.Repositories.Utilisateurs
// Namespace regroupant les interfaces et classes liées au repository Utilisateur.
{
    public interface IUtilisateurRepository
        : IGenericReadRepository<int, Utilisateur>, IGenericWriteRepository<int, Utilisateur>
    // Interface du repository Utilisateur :
    // - Hérite des opérations génériques de lecture (read)
    // - Hérite des opérations génériques d'écriture (write)
    // => CRUD complet via les interfaces génériques
    {
        // Ajouter ici des méthodes spécifiques au repository Utilisateurs si nécessaire

        Task<IEnumerable<Utilisateur>> GetUtilisateursByIdRecetteAsync(int idRecette);
        // Récupère tous les utilisateurs associés à une recette (ex : créateur).

        Task<bool> DeleteUtilisateurRelationsAsync(int idUtilisateur);
        // Méthode prévue pour supprimer les relations liées à un utilisateur.
        // Pas encore implémentée dans la classe concrète.

        Task<Utilisateur?> GetByIdentifiantAsync(string identifiant);
        // Récupère un utilisateur par son identifiant (login).
        // Retourne null si aucun utilisateur ne correspond.
    }
}
