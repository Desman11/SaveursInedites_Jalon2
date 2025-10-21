using SaveursInedites_Jalon2.DataAccessLayer.Unit_of_Work;
using SaveursInedites_Jalon2.Domain.BO;

namespace SaveursInedites_Jalon2.Services
{
    public class SaveursService : ISaveursService
    {
        private readonly IUoW _UoW;

        public SaveursService(IUoW UoW)
        {
            _UoW = UoW;
        }

        #region Gestion des recettes

        public async Task<IEnumerable<Recette>> GetAllRecettesAsync()
        {
            return await _UoW.Recette.GetAllAsync();
        }

        public async Task<Recette> GetRecetteByIdAsync(int id)
        {
            return await _UoW.Recette.GetAsync(id);
        }

        public async Task<Recette> AddRecetteAsync(Recette newRecette)
        {
            return await _UoW.Recette.CreateAsync(newRecette);
        }

        public async Task<Recette> ModifyRecetteAsync(Recette updateRecette)
        {
            return await _UoW.Recette.ModifyAsync(updateRecette);
        }

        public async Task<bool> DeleteRecetteAsync(int id)
        {
            return await _UoW.Recette.DeleteAsync(id);
        }

        #endregion Gestion des recettes

        #region Gestion des utilisateurs

        public async Task<IEnumerable<Utilisateur>> GetAllUtilisateursAsync()
        {
            return await _UoW.Utilisateur.GetAllAsync();
        }

        public async Task<Utilisateur> GetUtilisateurByIdAsync(int id)
        {
            return await _UoW.Utilisateur.GetAsync(id);
        }

        public async Task<Utilisateur> AddUtilisateurAsync(Utilisateur newUtilisateur)
        {
            return await _UoW.Utilisateur.CreateAsync(newUtilisateur);
        }

        public async Task<Utilisateur> ModifyUtilisateurAsync(Utilisateur updateUtilisateur)
        {
            return await _UoW.Utilisateur.ModifyAsync(updateUtilisateur);
        }

        public async Task<bool> DeleteUtilisateurAsync(int id)
        {
            return await _UoW.Utilisateur.DeleteAsync(id);
        }

        #endregion Gestion des auteurs

        #region Gestion des ingredients
        /// <summary>
        /// Récupère tous les ingredients.
        /// </summary>
        /// <returns>Une liste d'ingrédients.</returns>
        public async Task<IEnumerable<Ingredient>> GetAllIngredientsAsync()
        {
            return await _UoW.Ingredient.GetAllAsync();
        }

        /// <summary>
        /// Récupère un ingrédient par son identifiant.
        /// </summary>
        /// <param name="id">Identifiant de l'ingrédient.</param>
        /// <returns>L'ingrédient correspondant à l'identifiant.</returns>
        public async Task<Ingredient> GetIngredientByIdAsync(int id)
        {
            return await _UoW.Ingredient.GetAsync(id);
        }

        /// <summary>
        /// Ajoute un nouvel ingrédient.
        /// </summary>
        /// <param name="newIngredient">Ingrédient à ajouter.</param>
        /// <returns>L'ingrédient ajouté.</returns>
        public async Task<Ingredient> AddIngredientAsync(Ingredient newIngredient)
        {
            return await _UoW.Ingredient.CreateAsync(newIngredient);
        }

        /// <summary>
        /// Modifie un ingrédient existant.
        /// </summary>
        /// <param name="updateIngredient">Ingrédient à modifier.</param>
        /// <returns>L'ingrédient modifié.</returns>
        public async Task<Ingredient> ModifyIngredientAsync(Ingredient updateIngredient)
        {
            return await _UoW.Ingredient.ModifyAsync(updateIngredient);
        }

        /// <summary>
        /// Supprime un ingrédient par son identifiant.
        /// </summary>
        /// <param name="id">Identifiant de l'ingrédient à supprimer.</param>
        /// <returns>Vrai si la suppression a réussi, sinon faux.</returns>
        public async Task<bool> DeleteIngredientAsync(int id)
        {
            return await _UoW.Ingredient.DeleteAsync(id);
        }

        #endregion Gestion des ingrédients

        #region Gestion des relations entre recettes et ingredients

        /// <summary>
        /// Ajoute une relation entre une recette et un ingrédient.
        /// </summary>
        /// <param name="idIngredient">Identifiant de l'ingrédient.</param>
        /// <param name="idRecette">Identifiant de la recette.</param>
        /// <returns>Vrai si la relation a été ajoutée, sinon faux.</returns>
        public async Task<bool> AddRecetteIngredientRelationshipAsync(int idIngredient, int idRecette)
        {
            return await _UoW.Recette.AddRecetteIngredientRelationshipAsync(idIngredient, idRecette);
        }

        /// <summary>
        /// Supprime une relation entre une recette et un ingrédient.
        /// </summary>
        /// <param name="idIngredient">Identifiant de l'ingrédient.</param>
        /// <param name="idRecette">Identifiant de la recette.</param>
        /// <returns>Vrai si la relation a été supprimée, sinon faux.</returns>
        public async Task<bool> RemoveRecetteIngredientRelationshipAsync(int idIngredient, int idRecette)
        {
            return await _UoW.Recette.RemoveRecetteIngredientRelationshipAsync(idIngredient, idRecette);
        }

        /// <summary>
        /// Récupère la liste des recettes associées à un ingrédient.
        /// </summary>
        /// <param name="idIngredient">Identifiant de l'ingrédient.</param>
        /// <returns>Liste des recettes liées à l'ingrédient.</returns>
        public async Task<IEnumerable<Recette>> GetRecettesByIdIngredientAsync(int idIngredient)
        {
            return await _UoW.Recette.GetRecettesByIdIngredientAsync(idIngredient);
        }

        /// <summary>
        /// Récupère la liste des recettes associés à un        .
        /// </summary>
        /// <param name="idIngredient">Identifiant de l'ingredient.</param>
        /// <returns>Liste des recettes liées à l'ingredient.</returns>
        public async Task<IEnumerable<Recette>> GetRecetteByIdIngredientAsync(int idIngredient)
        {
            return await _UoW.Recette.GetRecettesByIdIngredientAsync(idIngredient);
        }

        /// <summary>
        /// Récupère la liste des ingredients  associés à une recette.
        /// </summary>
        /// <param name="idRecette">Identifiant de la recette.</param>
        /// <returns>Liste des ingredients liés à la recette.</returns>
        public async Task<IEnumerable<Ingredient>> GetIngredientsByIdRecetteAsync(int idRecette)
        {
            return await _UoW.Ingredient.GetIngredientByIdRecetteAsync(idRecette);
        }

        /// <summary>
        /// Supprime toutes les relations d'une recette avec des ingredients.
        /// </summary>
        /// <param name="idRecette">Identifiant de la recette.</param>
        /// <returns>Vrai si les relations ont été supprimées, sinon faux.</returns>
        public async Task<bool> DeleteRecetteRelationsAsync(int idRecette   )
        {
            return await _UoW.Recette.DeleteRecetteRelationsAsync(idRecette);
        }

        /// <summary>
        /// Supprime toutes les relations d'un ingredient avec des recettes.
        /// </summary>
        /// <param name="idIngredient">Identifiant de l'ingrédient.</param>
        /// <returns>Vrai si les relations ont été supprimées, sinon faux.</returns>
        public async Task<bool> DeleteIngredientRelationsAsync(int idIngredient)
        {
            return await _UoW.Ingredient.DeleteIngredientRelationsAsync(idIngredient);
        }

        #endregion Gestion des relations entre recettes et ingredients
}
    }

