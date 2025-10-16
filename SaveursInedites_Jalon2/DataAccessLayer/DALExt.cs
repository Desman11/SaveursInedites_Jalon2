using SaveursInedites_Jalon2.DataAccessLayer.Repositories.Utilisateurs;
using SaveursInedites_Jalon2.DataAccessLayer.Repositories.Recettes;
using SaveursInedites_Jalon2.DataAccessLayer.Repositories.Ingredients;
using SaveursInedites_Jalon2.DataAccessLayer.Session;
using SaveursInedites_Jalon2.DataAccessLayer.Session.MariaDB;
using SaveursInedites_Jalon2.DataAccessLayer.Session.MySQL;
using SaveursInedites_Jalon2.DataAccessLayer.Session.PostGres;
using SaveursInedites_Jalon2.DataAccessLayer.Unit_of_Work;
using SaveursInedites_Jalon2.Domain;


namespace SaveursInedites_Jalon2.DataAccessLayer
{
    public static class DalExt
    {
        public static void AddDal(this IServiceCollection services, IDatabaseSettings settings)
        {
            switch (settings.DatabaseProviderName)
            {
                case DatabaseProviderName.MariaDB:
                    services.AddScoped<IDBSession, MariaDBSession>();
                    break;
                case DatabaseProviderName.MySQL:
                    services.AddScoped<IDBSession, MySQLDBSession>();
                    break;
                case DatabaseProviderName.PostgreSQL:
                    services.AddScoped<IDBSession, PostGresDBSession>();
                    break;
            }

            services.AddScoped<IUoW, UoW>();
            services.AddTransient<IRecetteRepository, RecetteRepository>();
            services.AddTransient<IUtilisateurRepository, UtilisateurRepository>();
            services.AddTransient<IIngredientsRepository, IngredientsRepository>();
        }
    }
}
