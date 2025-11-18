namespace SaveursInedites_Jalon2.DataAccessLayer.Repositories
// Namespace contenant les interfaces et classes liées aux repositories de la DAL.
{
    public interface IGenericWriteRepository<TKey, TEntity>
    // Interface générique définissant les opérations d’écriture communes à tous les repositories.
    // TKey   : type de la clé primaire (ex. int, Guid)
    // TEntity : type de l’entité métier manipulée (ex. Recette, Utilisateur)
    {
        Task<TEntity> CreateAsync(TEntity entity);
        // Insère une nouvelle entité en base de données et renvoie l’entité créée.

        Task<TEntity> ModifyAsync(TEntity entity);
        // Met à jour une entité existante et renvoie l’entité mise à jour.

        Task<bool> DeleteAsync(TKey key);
        // Supprime une entité identifiée par sa clé primaire.
        // Retourne true si la suppression a réussi.
    }
}
