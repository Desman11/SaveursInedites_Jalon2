using SaveursInedites_Jalon2.DataAccessLayer.Repositories.Utilisateurs;
using SaveursInedites_Jalon2.DataAccessLayer.Repositories.Recettes;
using SaveursInedites_Jalon2.DataAccessLayer.Repositories.Ingredients;
using SaveursInedites_Jalon2.DataAccessLayer.Session;
using SaveursInedites_Jalon2.DataAccessLayer.Session.MariaDB;
using SaveursInedites_Jalon2.DataAccessLayer.Session.MySQL;
using SaveursInedites_Jalon2.DataAccessLayer.Session.PostGres;
using SaveursInedites_Jalon2.DataAccessLayer.Unit_of_Work;
using SaveursInedites_Jalon2.Domain;
// Imports nécessaires pour accéder aux différentes implémentations
// des sessions, repositories et au modèle de configuration BDD.

namespace SaveursInedites_Jalon2.DataAccessLayer
// Namespace dédié à la couche d'accès aux données (DAL).
{
    public static class DalExt
    // Classe statique fournissant une méthode d’extension
    // pour enregistrer les composants DAL dans l’injection de dépendances.
    {
        public static void AddDal(this IServiceCollection services, IDatabaseSettings settings)
        // Méthode d’extension utilisée pour enregistrer la DAL dans le conteneur DI.
        // Elle dépend de la configuration de la base (provider choisi).
        {
            switch (settings.DatabaseProviderName)
            // Sélectionne le provider de base de données et enregistre la session correspondante.
            {
                case DatabaseProviderName.MariaDB:
                    services.AddScoped<IDBSession, MariaDBSession>();
                    // Utilisation de MariaDB.
                    break;

                case DatabaseProviderName.MySQL:
                    services.AddScoped<IDBSession, MySQLDBSession>();
                    // Utilisation de MySQL.
                    break;

                case DatabaseProviderName.PostgreSQL:
                    services.AddScoped<IDBSession, PostGresDBSession>();
                    // Utilisation de PostgreSQL.
                    break;
            }

            services.AddScoped<IUoW, UoW>();
            // Enregistrement de l’Unit of Work.

            services.AddTransient<IRecetteRepository, RecetteRepository>();
            // Repository gérant les recettes.

            services.AddTransient<IIngredientRepository, IngredientRepository>();
            // Repository gérant les ingrédients.

            services.AddTransient<IUtilisateurRepository, UtilisateurRepository>();
            // Repository gérant les utilisateurs.
        }
    }
}
