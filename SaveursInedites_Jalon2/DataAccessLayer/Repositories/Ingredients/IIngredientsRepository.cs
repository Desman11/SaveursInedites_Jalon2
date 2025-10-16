using SaveursInedites_Jalon2.DataAccessLayer.Repositories;
using SaveursInedites_Jalon2.Domain.BO;

namespace SaveursInedites_Jalon2.DataAccessLayer.Repositories.Ingredients
{
    public interface IIngredientsRepository : IGenericReadRepository<int, Ingredients>, IGenericWriteRepository<int, Ingredients>
    {
        // Ajouter ici des méthodes spécifiques au repository Ingredients si nécessaire
        Task<IEnumerable<Ingredients>> GetIngredientsByIdRecipeAsync(int idRecipe);
        Task<bool> DeleteIngredientRelationsAsync(int idIngredient);
    }
}
