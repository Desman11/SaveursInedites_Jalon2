using SaveursInedites_Jalon2.DataAccessLayer.Repositories.Utilisateurs;
using SaveursInedites_Jalon2.DataAccessLayer.Repositories.Recettes;

namespace SaveursInedites_Jalon2.DataAccessLayer.Unit_of_Work
{
    public interface IUoW
    {
        #region Repositories

        IUtilisateurRepository Authors { get; }
        IRecetteRepository Books { get; }

        #endregion

        #region Transactions

        bool HasActiveTransaction { get; }

        void BeginTransaction();
        void Commit();
        void Rollback();

        #endregion Transactions
    }
}
