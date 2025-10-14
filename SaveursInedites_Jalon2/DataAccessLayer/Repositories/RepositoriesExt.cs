using SaveursInedites_Jalon2.DataAccessLayer.Repositories.Recettes;
using SaveursInedites_Jalon2.DataAccessLayer.Repositories.Utilisateurs;

namespace SaveursInedites_Jalon2.DataAccessLayer.Repositories
{
    public static class RepositoriesExt
    {
        public static void AddDal(this IServiceCollection services)
        {
            services.AddTransient<IRecetteRepository, RecetteRepository>();
            services.AddTransient<UtilisateurRepository, UtilisateurRepository>();
        }
    }
}
