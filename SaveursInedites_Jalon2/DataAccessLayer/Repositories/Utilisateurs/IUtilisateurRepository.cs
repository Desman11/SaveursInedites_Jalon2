using SaveursInedites_Jalon2.Domain.BO;

namespace SaveursInedites_Jalon2.DataAccessLayer.Repositories.Utilisateurs
{
    public interface IUtilisateurRepository : IGenericReadRepository<int, Utilisateur>, IGenericWriteRepository<int, Utilisateur>
    {
        // Ajouter ici des méthodes spécifiques au repository Utilisateurs si nécessaire
        Task<IEnumerable<Utilisateur>> GetUtilisateursByIdRecetteAsync(int idRecette);

        
    }
}


