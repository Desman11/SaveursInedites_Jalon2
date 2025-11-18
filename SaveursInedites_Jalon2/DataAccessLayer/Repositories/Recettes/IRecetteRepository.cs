using SaveursInedites_Jalon2.Domain.BO;
// Import du modèle métier Recette.

namespace SaveursInedites_Jalon2.DataAccessLayer.Repositories.Recettes
// Namespace regroupant les interfaces et classes du repository Recette.
{
    public interface IRecetteRepository
        : IGenericReadRepository<int, Recette>, IGenericWriteRepository<int, Recette>
    // Interface du repository Recette :
    // Hérite des opérations génériques de lecture/écriture pour fournir un CRUD complet.
    {
        // Ajouter ici des méthodes spécifiques au repository Recette si nécessaire

        Task<bool> AddRecetteUtilisateurRelationshipAsync(int idUtilisateur, int idRecette);
        // Ajoute une relation entre un utilisateur (créateur ou associé) et une recette.

        Task<bool> RemoveRecetteUtilisateurRelationshipAsync(int idUtilisateur, int idRecette);
        // Supprime une relation entre utilisateur et recette.

        Task<IEnumerable<Recette>> GetRecettesByIdUtilisateurAsync(int idUtilisateur);
        // Récupère toutes les recettes associées à un utilisateur.

        Task<bool> DeleteRecetteRelationsAsync(int idRecette);
        // Supprime toutes les relations entre une recette et les utilisateurs.

        Task<bool> AddRecetteIngredientRelationshipAsync(int idIngredient, int idRecette);
        // Ajoute une relation entre un ingrédient et une recette (table de liaison).

        Task<bool> RemoveRecetteIngredientRelationshipAsync(int idIngredient, int idRecette);
        // Supprime une relation recette–ingrédient.

        Task<IEnumerable<Recette>> GetRecettesByIdIngredientAsync(int idIngredient);
        // Récupère toutes les recettes associées à un ingrédient spécifique.
    }
}
