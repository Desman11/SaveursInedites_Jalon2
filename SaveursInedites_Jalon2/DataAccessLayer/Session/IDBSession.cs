using System.Data;
using SaveursInedites_Jalon2.Domain;

namespace SaveursInedites_Jalon2.DataAccessLayer.Session

{
    public interface IDBSession : IDisposable
    {
        DatabaseProviderName? DatabaseProviderName { get; }
        IDbConnection Connection { get; }
        IDbTransaction Transaction { get; }
        bool HasActiveTransaction { get; }

        void BeginTransaction();
        void Commit();
        void Rollback();
    }
}

