using MySql.Data.MySqlClient;
using System.Data;
using SaveursInedites_Jalon2.Domain;

namespace SaveursInedites_Jalon2.DataAccessLayer.Session.MySQL
{
    public class MySQLDBSession : BaseSession
    {
        public MySQLDBSession(IDatabaseSettings settings)
        {
            Connection = new MySqlConnection(settings.ConnectionString);
            DatabaseProviderName = settings.DatabaseProviderName;
        }
    }
}
