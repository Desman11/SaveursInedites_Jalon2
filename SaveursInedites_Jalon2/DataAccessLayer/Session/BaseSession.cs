using System.Data;
using SaveursInedites_Jalon2.Domain;

namespace SaveursInedites_Jalon2.DataAccessLayer.Session
// Namespace contenant les sessions de connexion et de transaction pour la DAL.
{
    public abstract class BaseSession : IDBSession
    // Classe abstraite servant de base aux différentes sessions (PostgreSQL, MySQL, etc.).
    // Elle implémente les comportements communs : gestion de connexion, transaction, IDisposable.
    {
        public DatabaseProviderName? DatabaseProviderName { get; protected set; }
        // Type de provider BDD utilisé (PostgreSQL, MySQL...). Défini par les classes dérivées.

        public IDbConnection Connection { get; protected set; }
        // Connexion sous-jacente à la base. Fourni par les classes concrètes.

        public IDbTransaction Transaction { get; private set; }
        // Transaction actuellement active, ou null si aucune transaction n’est ouverte.

        public bool HasActiveTransaction => Transaction != null;
        // Indique si une transaction est en cours.

        public void BeginTransaction()
        // Démarre une transaction si aucune n’existe déjà.
        {
            if (Transaction == null)
            {
                if (Connection?.State != ConnectionState.Open)
                    Connection?.Open();
                // Ouvre la connexion si elle ne l’est pas déjà.

                Transaction = Connection?.BeginTransaction();
                // Démarre la transaction sur la connexion.
            }
        }

        public void Commit()
        // Valide la transaction active.
        {
            Transaction?.Commit();
            Transaction = null;
            Connection?.Close();
            // Après un commit, la transaction est annulée et la connexion fermée.
        }

        public void Rollback()
        // Annule la transaction active.
        {
            Transaction?.Rollback();
            Transaction = null;
            Connection?.Close();
            // Comme pour Commit, on ferme la connexion après rollback.
        }

        #region IDisposable Support

        private bool _disposed = false;
        // Pour éviter de libérer deux fois les ressources.

        public void Dispose()
        // Libération explicite via IDisposable.
        {
            Dispose(true);
            GC.SuppressFinalize(this);
            // Empêche l'appel du finalizer.
        }

        protected virtual void Dispose(bool disposing)
        // Méthode virtuelle permettant aux classes dérivées de libérer leurs ressources.
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    // Libération des ressources managées.
                    Transaction?.Dispose();
                    Connection?.Dispose();
                }

                // Libération des ressources non managées (si besoin).

                _disposed = true;
            }
        }

        #endregion IDisposable Support
    }
}
