using SaveursInedites_Jalon2.Domain.BO;

namespace SaveursInedites_Jalon2.DataAccessLayer.Repositories.Recettes
{
    public interface IRecetteRepository : IGenericReadRepository<int, Recette>, IGenericWriteRepository<int, Recette>
    {
        // Ajouter ici des méthodes spécifiques au repository Recette si nécessaire

       

      

        Task<IEnumerable<Recette>> GetRecettesByIdUtilisateurAsync(int idUtilisateur);

      
    }
}

