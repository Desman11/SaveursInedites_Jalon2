using MySql.Data.MySqlClient;
using Npgsql;
using System.Data;
using System.Data.Common;
using SaveursInedites_Jalon2.Domain;
// Imports permettant d'accéder aux drivers BDD (Npgsql pour PostgreSQL) et aux types communs.

namespace SaveursInedites_Jalon2.DataAccessLayer.Session.PostGres
// Namespace contenant les implémentations spécifiques aux sessions PostgreSQL.
{
    public class PostGresDBSession : BaseSession
    // Session spécialisée pour PostgreSQL, héritant du comportement commun défini dans BaseSession.
    {
        public PostGresDBSession(IDatabaseSettings settings)
        // Constructeur recevant la configuration BDD (chaîne de connexion + provider).
        {
            Connection = new NpgsqlConnection(settings.ConnectionString);
            // Création d'une connexion PostgreSQL via le driver Npgsql.

            DatabaseProviderName = settings.DatabaseProviderName;
            // Mémorise le type de provider utilisé.
        }
    }
}
