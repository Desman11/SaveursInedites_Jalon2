using MySql.Data.MySqlClient;
using Npgsql;
using System.Data;
using System.Data.Common;
using SaveursInedites_Jalon2.Domain;

namespace SaveursInedites_Jalon2.DataAccessLayer.Session.PostGres
{
    public class PostGresDBSession : BaseSession
    {
        public PostGresDBSession(IDatabaseSettings settings)
        {
            Connection = new NpgsqlConnection(settings.ConnectionString);
            DatabaseProviderName = settings.DatabaseProviderName;
        }
    }
}
