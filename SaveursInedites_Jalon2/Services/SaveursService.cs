using SaveursInedites_Jalon2.DataAccessLayer.Unit_of_Work;
using SaveursInedites_Jalon2.Domain.BO;

namespace SaveursInedites_Jalon2.Services
{
    public class SaveursService : ISaveursService
    {
        private readonly IUoW _UoW;

        public SaveursService(IUoW UoW)
        {
            _UoW = UoW;
        }

        #region Gestion des recettes

        public async Task<IEnumerable<Recette>> GetAllRecettesAsync()
        {
            return await _UoW.Recettes.GetAllAsync();
        }

        public async Task<Recette> GetRecetteByIdAsync(int id)
        {
            return await _UoW.Recettes.GetAsync(id);
        }

        public async Task<Recette> AddRecetteAsync(Recette newRecette)
        {
            return await _UoW.Recettes.CreateAsync(newRecette);
        }

        public async Task<Recette> ModifyRecetteAsync(Recette updateRecette)
        {
            return await _UoW.Recettes.ModifyAsync(updateRecette);
        }

        public async Task<bool> DeleteRecetteAsync(int id)
        {
            return await _UoW.Recettes.DeleteAsync(id);
        }

        #endregion Gestion des recettes

        #region Gestion des utilisateurs

        public async Task<IEnumerable<Utilisateur>> GetAllUtilisateursAsync()
        {
            return await _UoW.Utilisateurs.GetAllAsync();
        }

        public async Task<Utilisateur> GetUtilisateurByIdAsync(int id)
        {
            return await _UoW.Utilisateurs.GetAsync(id);
        }

        public async Task<Utilisateur> AddUtilisateurAsync(Utilisateur newUtilisateur)
        {
            return await _UoW.Utilisateurs.CreateAsync(newUtilisateur);
        }

        public async Task<Utilisateur> ModifyUtilisateurAsync(Utilisateur updateUtilisateur)
        {
            return await _UoW.Utilisateurs.ModifyAsync(updateUtilisateur);
        }

        public async Task<bool> DeleteUtilisateurAsync(int id)
        {
            return await _UoW.Utilisateurs.DeleteAsync(id);
        }

        #endregion Gestion des auteurs

     
    }
}
