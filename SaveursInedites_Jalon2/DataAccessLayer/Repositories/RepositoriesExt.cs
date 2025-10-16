using SaveursInedites_Jalon2.DataAccessLayer.Repositories.Recettes;
using SaveursInedites_Jalon2.DataAccessLayer.Repositories.Utilisateurs;
using SaveursInedites_Jalon2.DataAccessLayer.Repositories.Ingredients;

namespace SaveursInedites_Jalon2.DataAccessLayer.Repositories
{
    public static class RepositoriesExt
    {
        public static void AddDal(this IServiceCollection services)
        {
            services.AddTransient<IRecetteRepository, RecetteRepository>();
            services.AddTransient<UtilisateurRepository, UtilisateurRepository>();
            services.AddTransient<IIngredientsRepository, IngredientsRepository>();
        }
    }
}
