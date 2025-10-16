using SaveursInedites_Jalon2.Domain.BO;

namespace SaveursInedites_Jalon2.Services
{
    public interface ISaveursService
    {
        #region Recettes

        Task<IEnumerable<Recette>> GetAllRecettesAsync();
        Task<Recette> GetRecetteByIdAsync(int id);
        Task<Recette> AddRecetteAsync(Recette newRecette);
        Task<Recette> ModifyRecetteAsync(Recette updateRecette);
        Task<bool> DeleteRecetteAsync(int id);

        #endregion Recettes

        #region  Utilisateurs

        Task<IEnumerable<Utilisateur>> GetAllUtilisateursAsync();
        Task<Utilisateur> GetUtilisateurByIdAsync(int id);
        Task<Utilisateur> AddUtilisateurAsync(Utilisateur newUtilisateur);
        Task<Utilisateur> ModifyUtilisateurAsync(Utilisateur updateUtilisateur);
        Task<bool> DeleteUtilisateurAsync(int id);

        #endregion  Utilisateurs

        #region Ingredients

        Task<IEnumerable<Ingredients>> GetAllIngredientsAsync();
        Task<Ingredients> GetIngredientByIdAsync(int id);
        Task<Ingredients> AddIngredientAsync(Ingredients newIngredient);
        Task<Ingredients> ModifyIngredientAsync(Ingredients updateIngredient);
        Task<bool> DeleteIngredientAsync(int id);

        #endregion Ingredients
    }
}

