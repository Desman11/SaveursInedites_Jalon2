using SaveursInedites_Jalon2.Domain.BO;

namespace SaveursInedites_Jalon2.Services
{
    public interface ISaveursService
    {
        // Recettes
        Task<IEnumerable<Recette>> GetAllRecettesAsync();
        Task<Recette?> GetRecetteByIdAsync(int id);
        Task<Recette> AddRecetteAsync(Recette newRecette);
        Task<Recette> ModifyRecetteAsync(Recette updateRecette);
        Task<bool> DeleteRecetteAsync(int id);

        // Utilisateurs
        Task<IEnumerable<Utilisateur>> GetAllUtilisateursAsync();
        Task<Utilisateur?> GetUtilisateurByIdAsync(int id);
        Task<Utilisateur> AddUtilisateurAsync(Utilisateur newUtilisateur);
        Task<Utilisateur> ModifyUtilisateurAsync(Utilisateur updateUtilisateur);
        Task<bool> DeleteUtilisateurAsync(int id);
        Task<Utilisateur?> GetUtilisateurByIdentifiantAsync(string identifiant);

        // Ingrédients
        Task<IEnumerable<Ingredient>> GetAllIngredientsAsync();
        Task<Ingredient?> GetIngredientByIdAsync(int id);
        Task<Ingredient> AddIngredientAsync(Ingredient newIngredient);
        Task<Ingredient> ModifyIngredientAsync(Ingredient updateIngredient);
        Task<bool> DeleteIngredientAsync(int id);

        // Relations Recette ↔ Ingrédient
        Task<bool> AddRecetteIngredientRelationshipAsync(int idIngredient, int idRecette);
        Task<bool> RemoveRecetteIngredientRelationshipAsync(int idIngredient, int idRecette);
        Task<IEnumerable<Recette>> GetRecettesByIdIngredientAsync(int idIngredient);
        Task<IEnumerable<Ingredient>> GetIngredientsByIdRecetteAsync(int idRecette);
        Task<bool> DeleteRecetteRelationsAsync(int idRecette);
        Task<bool> DeleteIngredientRelationsAsync(int idIngredient);
    }
}
