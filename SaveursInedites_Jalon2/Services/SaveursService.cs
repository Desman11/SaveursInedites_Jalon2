using SaveursInedites_Jalon2.DataAccessLayer.Unit_of_Work;
using SaveursInedites_Jalon2.Domain.BO;

namespace SaveursInedites_Jalon2.Services
{
    public class SaveursService : ISaveursService
    {
        private readonly IUoW _uow;

        public SaveursService(IUoW uow)
        {
            _uow = uow;
        }

        #region Gestion des recettes

        public async Task<IEnumerable<Recette>> GetAllRecettesAsync()
            => await _uow.Recettes.GetAllAsync();

        public async Task<Recette?> GetRecetteByIdAsync(int id)
            => await _uow.Recettes.GetAsync(id);

        public async Task<Recette> AddRecetteAsync(Recette newRecette)
            => await _uow.Recettes.CreateAsync(newRecette);

        public async Task<Recette> ModifyRecetteAsync(Recette updateRecette)
            => await _uow.Recettes.ModifyAsync(updateRecette);

        public async Task<bool> DeleteRecetteAsync(int id)
            => await _uow.Recettes.DeleteAsync(id);

        #endregion

        #region Gestion des utilisateurs

        public async Task<IEnumerable<Utilisateur>> GetAllUtilisateursAsync()
            => await _uow.Utilisateurs.GetAllAsync();

        public async Task<Utilisateur?> GetUtilisateurByIdAsync(int id)
            => await _uow.Utilisateurs.GetAsync(id);

        public async Task<Utilisateur> AddUtilisateurAsync(Utilisateur newUtilisateur)
            => await _uow.Utilisateurs.CreateAsync(newUtilisateur);

        public async Task<Utilisateur> ModifyUtilisateurAsync(Utilisateur updateUtilisateur)
            => await _uow.Utilisateurs.ModifyAsync(updateUtilisateur);

        public async Task<bool> DeleteUtilisateurAsync(int id)
            => await _uow.Utilisateurs.DeleteAsync(id);

        #endregion

        #region Gestion des ingrédients

        public async Task<IEnumerable<Ingredient>> GetAllIngredientsAsync()
            => await _uow.Ingredients.GetAllAsync();

        public async Task<Ingredient?> GetIngredientByIdAsync(int id)
            => await _uow.Ingredients.GetAsync(id);

        public async Task<Ingredient> AddIngredientAsync(Ingredient newIngredient)
            => await _uow.Ingredients.CreateAsync(newIngredient);

        public async Task<Ingredient> ModifyIngredientAsync(Ingredient updateIngredient)
            => await _uow.Ingredients.ModifyAsync(updateIngredient);

        public async Task<bool> DeleteIngredientAsync(int id)
            => await _uow.Ingredients.DeleteAsync(id);

        #endregion

        #region Gestion des relations Recette ↔ Ingrédient

        public async Task<bool> AddRecetteIngredientRelationshipAsync(int idIngredient, int idRecette)
            => await _uow.Recettes.AddRecetteIngredientRelationshipAsync(idIngredient, idRecette);

        public async Task<bool> RemoveRecetteIngredientRelationshipAsync(int idIngredient, int idRecette)
            => await _uow.Recettes.RemoveRecetteIngredientRelationshipAsync(idIngredient, idRecette);

        public async Task<IEnumerable<Recette>> GetRecettesByIdIngredientAsync(int idIngredient)
            => await _uow.Recettes.GetRecettesByIdIngredientAsync(idIngredient);

        public async Task<IEnumerable<Ingredient>> GetIngredientsByIdRecetteAsync(int idRecette)
            // Selon ton DAL, cette méthode peut vivre côté Recettes ou Ingrédients.
            // Conserve celle qui existe réellement.
            => await _uow.Ingredients.GetIngredientsByIdRecetteAsync(idRecette);
        // => await _uow.Recettes.GetIngredientsByIdRecetteAsync(idRecette);

        public async Task<bool> DeleteRecetteRelationsAsync(int idRecette)
            => await _uow.Recettes.DeleteRecetteRelationsAsync(idRecette);

        public async Task<bool> DeleteIngredientRelationsAsync(int idIngredient)
            => await _uow.Ingredients.DeleteIngredientRelationsAsync(idIngredient);

        #endregion
    }
}
