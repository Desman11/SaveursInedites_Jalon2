using SaveursInedites_Jalon2.DataAccessLayer.Repositories.Recettes;
using SaveursInedites_Jalon2.DataAccessLayer.Repositories.Utilisateurs;
using SaveursInedites_Jalon2.DataAccessLayer.Repositories.Ingredients;

namespace SaveursInedites_Jalon2.DataAccessLayer.Unit_of_Work
{
    public interface IUoW
    {
        #region Repositories

        IRecetteRepository Recettes { get; }
        IUtilisateurRepository Utilisateurs { get; }
        IIngredientRepository Ingredients { get; }

        #endregion

        #region Transactions

        bool HasActiveTransaction { get; }

        void BeginTransaction();
        void Commit();
        void Rollback();

        #endregion Transactions
    }
}
