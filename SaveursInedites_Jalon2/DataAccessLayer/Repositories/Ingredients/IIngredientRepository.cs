using SaveursInedites_Jalon2.DataAccessLayer.Repositories;
using SaveursInedites_Jalon2.Domain.BO;

namespace SaveursInedites_Jalon2.DataAccessLayer.Repositories.Ingredients
{
    public interface IIngredientRepository : IGenericReadRepository<int, Ingredient>, IGenericWriteRepository<int, Ingredient>
    {
        // Ajouter ici des méthodes spécifiques au repository Ingredient si nécessaire
        Task<bool> AddIngredientToRecipeAsync(int idIngredient, int idRecipe);
        Task<bool> RemoveIngredientFromRecipeAsync(int idIngredient, int idRecipe);
        Task<IEnumerable<Ingredient>> GetIngredientsByIdRecipeAsync(int idRecipe);
        Task<bool> DeleteIngredientRelationsAsync(int idIngredient);
    }
}
