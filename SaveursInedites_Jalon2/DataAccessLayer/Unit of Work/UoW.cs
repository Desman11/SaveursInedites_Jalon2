using System.Data;
using System.Transactions;
using SaveursInedites_Jalon2.DataAccessLayer.Repositories.Utilisateurs;
using SaveursInedites_Jalon2.DataAccessLayer.Repositories.Recettes;
using SaveursInedites_Jalon2.DataAccessLayer.Repositories.Ingredients;
using SaveursInedites_Jalon2.DataAccessLayer.Session;

namespace SaveursInedites_Jalon2.DataAccessLayer.Unit_of_Work
{
    public class UoW
    {
        private readonly IDBSession _dbSession;
        private readonly Lazy<IUtilisateurRepository> _utilisateurs;
        private readonly Lazy<IRecetteRepository> _recettes;
        private readonly Lazy<IIngredientRepository> _ingredients;

        public UoW(IDBSession dbSession, IServiceProvider serviceProvider)
        {
            _dbSession = dbSession;
            _utilisateurs = new Lazy<IUtilisateurRepository>(() => serviceProvider.GetRequiredService<IUtilisateurRepository>());
            _recettes = new Lazy<IRecetteRepository>(() => serviceProvider.GetRequiredService<IRecetteRepository>());
            _ingredients = new Lazy<IIngredientRepository>(() => serviceProvider.GetRequiredService<IIngredientRepository>());    
        }

        #region Repositories

        // ATTENTION : Les repositories doivent utiliser la transaction en cours dans
        // les requêtes Dapper

        public IUtilisateurRepository Utilisateurs => _utilisateurs.Value;

        public IRecetteRepository Recettes => _recettes.Value;

        public IIngredientRepository Ingredients => _ingredients.Value;

        #endregion Repositories

        #region Transactions

        public bool HasActiveTransaction => _dbSession.HasActiveTransaction;

        public void BeginTransaction()
            => _dbSession.BeginTransaction();

        public void Commit()
            => _dbSession.Commit();

        public void Rollback()
            => _dbSession.Rollback();

        #endregion Transactions
    }
}

