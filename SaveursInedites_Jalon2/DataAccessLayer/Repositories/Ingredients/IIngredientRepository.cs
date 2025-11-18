using SaveursInedites_Jalon2.Domain.BO;
// Import du modèle métier Ingredient.

namespace SaveursInedites_Jalon2.DataAccessLayer.Repositories.Ingredients
// Namespace contenant les interfaces et classes du repository Ingredient.
{
    public interface IIngredientRepository
        : IGenericReadRepository<int, Ingredient>, IGenericWriteRepository<int, Ingredient>
    // Interface du repository Ingredient :
    // - Hérite du CRUD générique de lecture (Read)
    // - Hérite du CRUD générique d’écriture (Write)
    {
        // Ajouter ici des méthodes spécifiques au repository Ingredient si nécessaire

        Task<bool> AddIngredientRecetteRelationshipAsync(int idRecette, int idIngredient);
        // Ajoute une relation entre une recette et un ingrédient (table de liaison recette_ingredient).

        Task<bool> RemoveIngredientRecetteRelationshipAsync(int idRecette, int idIngredient);
        // Supprime la relation entre une recette et un ingrédient donné.

        Task<IEnumerable<Ingredient>> GetIngredientsByIdRecetteAsync(int idRecette);
        // Récupère tous les ingrédients associés à une recette.

        Task<bool> DeleteIngredientRelationsAsync(int idIngredient);
        // Supprime toutes les relations dans la table de liaison pour un ingrédient donné.
    }
}
