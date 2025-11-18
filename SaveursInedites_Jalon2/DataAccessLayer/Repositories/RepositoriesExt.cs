using SaveursInedites_Jalon2.DataAccessLayer.Repositories.Recettes;
using SaveursInedites_Jalon2.DataAccessLayer.Repositories.Utilisateurs;
using SaveursInedites_Jalon2.DataAccessLayer.Repositories.Ingredients;

// Imports des interfaces et implémentations des repositories liés aux recettes,
// utilisateurs et ingrédients.

namespace SaveursInedites_Jalon2.DataAccessLayer.Repositories
// Namespace contenant les extensions permettant d’enregistrer les repositories dans le DI container.
{
    public static class RepositoriesExt
    // Classe statique fournissant une méthode d’extension pour enregistrer les repositories.
    {
        public static void AddDal(this IServiceCollection services)
        // Méthode d’extension permettant d’ajouter les repositories à l’injection de dépendances.
        {
            services.AddTransient<IRecetteRepository, RecetteRepository>();
            // Enregistre l’implémentation du repository des recettes.
            // Transient : une nouvelle instance est créée à chaque demande.

            services.AddTransient<UtilisateurRepository, UtilisateurRepository>();
            // Enregistre le repository des utilisateurs.
            // Remarque : ici l’interface IUtilisateurRepository est absente pour l’enregistrement,
            // ce qui est probablement une erreur. Cela enregistre l’implémentation sous son propre type.

            services.AddTransient<IIngredientRepository, IngredientRepository>();
            // Enregistre l’implémentation du repository des ingrédients.
        }
    }
}
