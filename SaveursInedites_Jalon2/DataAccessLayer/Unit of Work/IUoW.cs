using SaveursInedites_Jalon2.DataAccessLayer.Repositories.Utilisateurs;
using SaveursInedites_Jalon2.DataAccessLayer.Repositories.Recettes;
using SaveursInedites_Jalon2.DataAccessLayer.Repositories.Ingredients;

namespace SaveursInedites_Jalon2.DataAccessLayer.Unit_of_Work
{
    public interface IUoW
    {
        #region Repositories

        IUtilisateurRepository Utilisateur { get; }
        IRecetteRepository Recette { get; }
        IIngredientRepository Ingredient { get; }

        #endregion

        #region Transactions

        bool HasActiveTransaction { get; }

        void BeginTransaction();
        void Commit();
        void Rollback();

        #endregion Transactions
    }
}
