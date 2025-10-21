﻿using SaveursInedites_Jalon2.Domain.BO;

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

        Task<IEnumerable<Ingredient>> GetAllIngredientsAsync();
        Task<Ingredient> GetIngredientByIdAsync(int id);
        Task<Ingredient> AddIngredientAsync(Ingredient newIngredient);
        Task<Ingredient> ModifyIngredientAsync(Ingredient updateIngredient);
        Task<bool> DeleteIngredientAsync(int id);

        #endregion Ingredients

        #region Relations Recettes - Ingredients

        Task<bool> AddRecetteIngredientRelationshipAsync(int idIngredient, int idRecette);
        Task<bool> RemoveRecetteIngredientRelationshipAsync(int idIngredient, int idRecette);
        Task<IEnumerable<Recette>> GetRecettesByIdIngredientAsync(int idIngredient);
       Task GetIngredientsByIdRecetteAsync(int idIngredient);
        Task<bool> DeleteRecetteRelationsAsync(int idRecette);
        Task<bool> DeleteIngredientRelationsAsync(int idIngredient);

        #endregion Relations Recettes - Ingredients

    }
}

