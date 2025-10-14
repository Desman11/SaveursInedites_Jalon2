using SaveursInedites_Jalon2.Domain.BO;

namespace SaveursInedites_Jalon2.DataAccessLayer.Repositories.Recettes
{
    public interface IRecetteRepository : IGenericReadRepository<int, Recette>, IGenericWriteRepository<int, Recette>
    {
        // Ajouter ici des méthodes spécifiques au repository Recette si nécessaire
        Task<bool> AddRecetteIngredientRelationshipAsync(int idIngredient, int idRecette);
        Task<bool> RemoveRecetteIngredientRelationshipAsync(int idIngredient, int idRecette);
        Task<IEnumerable<Recette>> GetRecettesByIdIngredientAsync(int idIngredient);
        Task<bool> DeleteRecetteRelationsAsync(int idRecette);
    }
}
