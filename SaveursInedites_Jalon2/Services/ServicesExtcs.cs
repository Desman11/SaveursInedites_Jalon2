namespace SaveursInedites_Jalon2.Services
// Déclare l’espace de noms contenant les services de l’application.
{
    public static class ServicesExt
    // Classe statique servant d’extension pour ajouter des services à l’injection de dépendances.
    {
        public static void AddBll(this IServiceCollection services)
        // Méthode d’extension permettant d’enregistrer les services métier (BLL) dans le conteneur DI.
        {
            services.AddTransient<ISaveursService, SaveursService>();
            // Enregistre ISaveursService avec l’implémentation SaveursService en mode Transient.
            // Une nouvelle instance sera créée à chaque demande.

            services.AddTransient<IJwtTokenService, JwtTokenService>();
            // Enregistre IJwtTokenService avec l’implémentation JwtTokenService.
            // Là encore, Transient crée une instance différente à chaque utilisation.
        }
    }
}
