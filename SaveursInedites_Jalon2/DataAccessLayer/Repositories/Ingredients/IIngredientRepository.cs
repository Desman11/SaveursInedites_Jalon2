using SaveursInedites_Jalon2.Domain.BO;

namespace SaveursInedites_Jalon2.DataAccessLayer.Repositories.Ingredients
{
    public interface IIngredientRepository : IGenericReadRepository<int, Ingredient>, IGenericWriteRepository<int, Ingredient>
    {
        // Ajouter ici des méthodes spécifiques au repository Ingredient si nécessaire
        Task<bool> AddIngredientRecetteRelationshipAsync(int idRecette, int idIngredient);
        Task<bool> RemoveIngredientRecetteRelationshipAsync(int idRecette, int idIngredient);
        Task<IEnumerable<Ingredient>> GetIngredientsByIdRecetteAsync(int idRecette);
        Task<bool> DeleteIngredientRelationsAsync(int idIngredient);
    }
}
