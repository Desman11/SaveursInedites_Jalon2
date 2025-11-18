using MySql.Data.MySqlClient;
using System.Data;
using SaveursInedites_Jalon2.Domain;
// Imports nécessaires pour créer une connexion MariaDB (via le driver MySql).

namespace SaveursInedites_Jalon2.DataAccessLayer.Session.MariaDB
// Namespace contenant les implémentations de sessions spécifiques à MariaDB.
{
    public class MariaDBSession : BaseSession
    // Session spécifique pour MariaDB, héritant de BaseSession.
    {
        public MariaDBSession(IDatabaseSettings settings)
        // Constructeur recevant les paramètres BDD (chaîne de connexion + provider).
        {
            Connection = new MySqlConnection(settings.ConnectionString);
            // MariaDB utilise le même driver MySqlConnection pour établir la connexion.

            DatabaseProviderName = settings.DatabaseProviderName;
            // Indique le type de base de données configuré.
        }
    }
}
