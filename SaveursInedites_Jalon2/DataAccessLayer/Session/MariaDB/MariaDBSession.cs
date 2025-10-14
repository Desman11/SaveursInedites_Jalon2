using MySql.Data.MySqlClient;
using System.Data;
using SaveursInedites_Jalon2.Domain;

namespace SaveursInedites_Jalon2.DataAccessLayer.Session.MariaDB
{
    public class MariaDBSession : BaseSession
    {
        public MariaDBSession(IDatabaseSettings settings)
        {
            Connection = new MySqlConnection(settings.ConnectionString);
            DatabaseProviderName = settings.DatabaseProviderName;
        }
    }
}
