using MySql.Data.MySqlClient;
using System.Data;
using SaveursInedites_Jalon2.Domain;
// Imports nécessaires pour manipuler une connexion MySQL et accéder aux paramètres BDD.

namespace SaveursInedites_Jalon2.DataAccessLayer.Session.MySQL
// Namespace regroupant les implémentations de session spécifiques à MySQL.
{
    public class MySQLDBSession : BaseSession
    // Session spécifique pour MySQL, héritant du comportement commun de BaseSession.
    {
        public MySQLDBSession(IDatabaseSettings settings)
        // Constructeur recevant les paramètres de configuration de la base.
        {
            Connection = new MySqlConnection(settings.ConnectionString);
            // Création d’une connexion MySQL via MySqlConnector.

            DatabaseProviderName = settings.DatabaseProviderName;
            // Enregistre le provider utilisé pour cette session.
        }
    }
}
