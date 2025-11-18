using System.Data;
// Fournit les interfaces et types de base pour manipuler les données (IDbConnection, IDbTransaction, etc.).

using SaveursInedites_Jalon2.DataAccessLayer.Repositories.Utilisateurs;
// Import du contrat et de l’implémentation des repositories liés aux utilisateurs.

using SaveursInedites_Jalon2.DataAccessLayer.Repositories.Recettes;
// Import du contrat et de l’implémentation des repositories liés aux recettes.

using SaveursInedites_Jalon2.DataAccessLayer.Repositories.Ingredients;
// Import du contrat et de l’implémentation des repositories liés aux ingrédients.

using SaveursInedites_Jalon2.DataAccessLayer.Session;
// Import du contrat IDBSession gérant la connexion et les transactions BDD.

namespace SaveursInedites_Jalon2.DataAccessLayer.Unit_of_Work
// Namespace qui regroupe l’implémentation du pattern Unit of Work (UoW).
{
    public class UoW : IUoW
    // Classe concrète de l’Unit of Work implémentant l’interface IUoW.
    {
        private readonly IDBSession _dbSession;
        // Session de base de données partagée (connexion + transaction éventuelle).

        private readonly Lazy<IUtilisateurRepository> _utilisateurs;
        // Repository des utilisateurs, instancié à la demande (Lazy).

        private readonly Lazy<IRecetteRepository> _recettes;
        // Repository des recettes, instancié à la demande.

        private readonly Lazy<IIngredientRepository> _ingredients;
        // Repository des ingrédients, instancié à la demande.

        public UoW(IDBSession dbSession, IServiceProvider serviceProvider)
        // Constructeur recevant la session de BDD et le conteneur de services (DI).
        {
            _dbSession = dbSession;
            // Stocke la session pour la gestion des transactions.

            _utilisateurs = new Lazy<IUtilisateurRepository>(
                () => serviceProvider.GetRequiredService<IUtilisateurRepository>());
            // Instancie le repository des utilisateurs via l’IoC container, seulement au premier accès.

            _recettes = new Lazy<IRecetteRepository>(
                () => serviceProvider.GetRequiredService<IRecetteRepository>());
            // Instancie le repository des recettes via le conteneur, à la demande.

            _ingredients = new Lazy<IIngredientRepository>(
                () => serviceProvider.GetRequiredService<IIngredientRepository>());
            // Instancie le repository des ingrédients via le conteneur, à la demande.
        }

        #region Repositories

        // ATTENTION : Les repositories doivent utiliser la transaction en cours dans
        // les requêtes Dapper
        // => Ils doivent travailler avec la même connexion / transaction gérées par IDBSession.

        public IUtilisateurRepository Utilisateurs => _utilisateurs.Value;
        // Accès au repository des utilisateurs (instanciation Lazy au premier appel).

        public IRecetteRepository Recettes => _recettes.Value;
        // Accès au repository des recettes.

        public IIngredientRepository Ingredients => _ingredients.Value;
        // Accès au repository des ingrédients.

        #endregion Repositories

        #region Transactions

        public bool HasActiveTransaction => _dbSession.HasActiveTransaction;
        // Indique s’il existe actuellement une transaction active sur la session.

        public void BeginTransaction()
            => _dbSession.BeginTransaction();
        // Démarre une nouvelle transaction en base via la session.

        public void Commit()
            => _dbSession.Commit();
        // Valide la transaction en cours (commit) via la session.

        public void Rollback()
            => _dbSession.Rollback();
        // Annule la transaction en cours (rollback) via la session.

        #endregion Transactions
    }
}
