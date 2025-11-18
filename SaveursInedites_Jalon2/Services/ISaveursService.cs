using SaveursInedites_Jalon2.Domain.BO;
// Importe les classes métier (BO : Business Objects) utilisées dans les signatures,
// notamment Recette, Utilisateur et Ingredient.

namespace SaveursInedites_Jalon2.Services
// Déclare l’espace de noms contenant les services de l’application.
{
    public interface ISaveursService
    // Interface définissant le contrat du service principal manipulant les entités métiers.
    {
        // Recettes -------------------------------------------------------------

        Task<IEnumerable<Recette>> GetAllRecettesAsync();
        // Retourne la liste complète des recettes.

        Task<Recette?> GetRecetteByIdAsync(int id);
        // Retourne une recette selon son identifiant, ou null si elle n’existe pas.

        Task<Recette> AddRecetteAsync(Recette newRecette);
        // Ajoute une nouvelle recette et renvoie l’entité persistée.

        Task<Recette> ModifyRecetteAsync(Recette updateRecette);
        // Modifie une recette existante et renvoie l’entité mise à jour.

        Task<bool> DeleteRecetteAsync(int id);
        // Supprime une recette via son identifiant. Retourne true si la suppression réussit.


        // Utilisateurs ---------------------------------------------------------

        Task<IEnumerable<Utilisateur>> GetAllUtilisateursAsync();
        // Retourne la liste complète des utilisateurs.

        Task<Utilisateur?> GetUtilisateurByIdAsync(int id);
        // Retourne un utilisateur selon son identifiant, ou null s’il n’existe pas.

        Task<Utilisateur> AddUtilisateurAsync(Utilisateur newUtilisateur);
        // Ajoute un nouvel utilisateur et renvoie l’entité persistée.

        Task<Utilisateur> ModifyUtilisateurAsync(Utilisateur updateUtilisateur);
        // Modifie un utilisateur existant et renvoie l’entité mise à jour.

        Task<bool> DeleteUtilisateurAsync(int id);
        // Supprime un utilisateur via son identifiant.

        Task<Utilisateur?> GetUtilisateurByIdentifiantAsync(string identifiant);
        // Recherche un utilisateur via son identifiant de connexion.


        // Ingrédients ----------------------------------------------------------

        Task<IEnumerable<Ingredient>> GetAllIngredientsAsync();
        // Retourne la liste complète des ingrédients.

        Task<Ingredient?> GetIngredientByIdAsync(int id);
        // Retourne un ingrédient selon son id, ou null s’il n’existe pas.

        Task<Ingredient> AddIngredientAsync(Ingredient newIngredient);
        // Ajoute un nouvel ingrédient dans la base.

        Task<Ingredient> ModifyIngredientAsync(Ingredient updateIngredient);
        // Modifie un ingrédient existant.

        Task<bool> DeleteIngredientAsync(int id);
        // Supprime un ingrédient via son identifiant.


        // Relations Recette ↔ Ingrédient ---------------------------------------

        Task<bool> AddRecetteIngredientRelationshipAsync(int idIngredient, int idRecette);
        // Crée la relation entre une recette et un ingrédient.

        Task<bool> RemoveRecetteIngredientRelationshipAsync(int idIngredient, int idRecette);
        // Supprime la relation entre une recette et un ingrédient.

        Task<IEnumerable<Recette>> GetRecettesByIdIngredientAsync(int idIngredient);
        // Retourne toutes les recettes utilisant un ingrédient donné.

        Task<IEnumerable<Ingredient>> GetIngredientsByIdRecetteAsync(int idRecette);
        // Retourne tous les ingrédients associés à une recette donnée.

        Task<bool> DeleteRecetteRelationsAsync(int idRecette);
        // Supprime toutes les relations entre une recette et ses ingrédients.

        Task<bool> DeleteIngredientRelationsAsync(int idIngredient);
        // Supprime toutes les relations entre un ingrédient et ses recettes.
    }
}
