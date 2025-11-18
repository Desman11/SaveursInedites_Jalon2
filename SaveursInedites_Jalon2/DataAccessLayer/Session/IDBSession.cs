using System.Data;
// Fournit les interfaces standard ADO.NET (IDbConnection, IDbTransaction, etc.).

using SaveursInedites_Jalon2.Domain;
// Permet d’accéder à l’énumération DatabaseProviderName.

namespace SaveursInedites_Jalon2.DataAccessLayer.Session
// Namespace contenant les composants liés à la gestion de la session BDD (connexion + transaction).
{
    public interface IDBSession : IDisposable
    // Interface représentant une session de base de données :
    // - Elle encapsule la connexion
    // - Elle gère une éventuelle transaction
    // - Elle peut être utilisée par plusieurs repositories via l’UoW
    {
        DatabaseProviderName? DatabaseProviderName { get; }
        // Indique quel type de fournisseur de base de données est utilisé
        // (PostgreSQL, MariaDB, MySQL, etc.).

        IDbConnection Connection { get; }
        // Connexion sous-jacente à la base de données (ouverte et réutilisée).

        IDbTransaction Transaction { get; }
        // Transaction en cours, ou null s’il n’y en a pas.

        bool HasActiveTransaction { get; }
        // Indique si une transaction est actuellement ouverte.

        void BeginTransaction();
        // Ouvre une transaction sur la connexion.

        void Commit();
        // Valide la transaction en cours.

        void Rollback();
        // Annule la transaction en cours.

        // Hérite de IDisposable :
        // Oblige l’implémentation à libérer proprement la connexion et la transaction.
    }
}
