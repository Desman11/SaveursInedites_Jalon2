namespace SaveursInedites_Jalon2.DataAccessLayer.Repositories
// Namespace regroupant les interfaces et classes de repositories de la DAL.
{
    public interface IGenericReadRepository<TKey, TEntity>
    // Interface générique définissant les opérations de lecture communes à tous les repositories.
    // TKey    : type de la clé primaire (ex. int, Guid)
    // TEntity : type de l’entité métier manipulée (ex. Recette, Utilisateur, Ingredient)
    {
        Task<IEnumerable<TEntity>> GetAllAsync();
        // Récupère l’ensemble des entités de ce type depuis la base de données.

        Task<TEntity> GetAsync(TKey key);
        // Récupère une entité unique à partir de sa clé primaire.
        // Peut lever une exception ou retourner null (selon l’implémentation) si l’entité n’existe pas.
    }
}
