using SaveursInedites_Jalon2.DataAccessLayer.Unit_of_Work;
// Import du namespace contenant l’interface IUoW (Unit of Work), qui regroupe les différents dépôts (repositories).

using SaveursInedites_Jalon2.Domain.BO;
// Import des objets métiers (BO) : Recette, Utilisateur, Ingredient, etc.

namespace SaveursInedites_Jalon2.Services
// Espace de noms qui regroupe les services de la couche métier (BLL) de l’application.
{
    public class SaveursService : ISaveursService
    // Classe de service principale qui implémente l’interface ISaveursService.
    // Elle expose les opérations métier sur les recettes, utilisateurs et ingrédients.
    {
        private readonly IUoW _uow;
        // Champ privé en lecture seule qui référence l’Unit of Work.
        // IUoW centralise l’accès aux différents repositories et la gestion de la persistance.

        public SaveursService(IUoW uow)
        // Constructeur recevant une instance d’IUoW via l’injection de dépendances.
        {
            _uow = uow;
            // Stocke l’instance d’Unit of Work dans le champ privé pour l’utiliser dans les méthodes du service.
        }

        #region Gestion des recettes
        // Région regroupant toutes les méthodes liées à la gestion des recettes.

        public async Task<IEnumerable<Recette>> GetAllRecettesAsync()
            // Méthode asynchrone qui retourne toutes les recettes.
            => await _uow.Recettes.GetAllAsync();
        // Délègue l’appel au repository de recettes via l’Unit of Work.

        public async Task<Recette?> GetRecetteByIdAsync(int id)
            // Méthode asynchrone qui récupère une recette par son identifiant.
            // Peut retourner null si aucune recette ne correspond.
            => await _uow.Recettes.GetAsync(id);
        // Appel du repository Recettes pour récupérer l’entité.

        public async Task<Recette> AddRecetteAsync(Recette newRecette)
            // Méthode asynchrone pour ajouter une nouvelle recette.
            // Retourne la recette créée (éventuellement avec son Id généré).
            => await _uow.Recettes.CreateAsync(newRecette);
        // Délègue la création au repository de recettes.

        public async Task<Recette> ModifyRecetteAsync(Recette updateRecette)
            // Méthode asynchrone pour modifier une recette existante.
            // Retourne la recette mise à jour après persistance.
            => await _uow.Recettes.ModifyAsync(updateRecette);
        // Appel du repository pour appliquer la mise à jour.

        public async Task<bool> DeleteRecetteAsync(int id)
            // Méthode asynchrone pour supprimer une recette via son identifiant.
            // Retourne true si la suppression a réussi, false sinon.
            => await _uow.Recettes.DeleteAsync(id);
        // Délègue la suppression au repository Recettes.

        #endregion

        #region Gestion des utilisateurs
        // Région regroupant la gestion des utilisateurs.

        public async Task<IEnumerable<Utilisateur>> GetAllUtilisateursAsync()
            // Méthode asynchrone qui retourne tous les utilisateurs.
            => await _uow.Utilisateurs.GetAllAsync();
        // Appel au repository Utilisateurs pour obtenir la liste.

        public async Task<Utilisateur?> GetUtilisateurByIdAsync(int id)
            // Récupère un utilisateur par son identifiant.
            // Peut renvoyer null si aucun utilisateur ne correspond.
            => await _uow.Utilisateurs.GetAsync(id);
        // Délègue au repository Utilisateurs.

        public async Task<Utilisateur> AddUtilisateurAsync(Utilisateur newUtilisateur)
            // Ajoute un nouvel utilisateur en base.
            // Retourne l’utilisateur créé.
            => await _uow.Utilisateurs.CreateAsync(newUtilisateur);
        // Appel de la méthode de création dans le repository Utilisateurs.

        public async Task<Utilisateur> ModifyUtilisateurAsync(Utilisateur updateUtilisateur)
            // Met à jour un utilisateur existant.
            // Retourne l’utilisateur après modification.
            => await _uow.Utilisateurs.ModifyAsync(updateUtilisateur);
        // Délègue la mise à jour au repository.

        public async Task<bool> DeleteUtilisateurAsync(int id)
            // Supprime un utilisateur par son identifiant.
            // Retourne true si la suppression a abouti.
            => await _uow.Utilisateurs.DeleteAsync(id);
        // Appel du repository pour effectuer la suppression.

        public async Task<Utilisateur?> GetUtilisateurByIdentifiantAsync(string identifiant)
        // Méthode asynchrone qui permet de récupérer un utilisateur
        // à partir de son identifiant (login / username).
        {
            return await _uow.Utilisateurs.GetByIdentifiantAsync(identifiant);
            // Délègue la recherche au repository Utilisateurs.
        }

        #endregion

        #region Gestion des ingrédients
        // Région dédiée aux opérations sur les ingrédients.

        public async Task<IEnumerable<Ingredient>> GetAllIngredientsAsync()
            // Retourne la liste de tous les ingrédients.
            => await _uow.Ingredients.GetAllAsync();
        // Appel au repository d’ingrédients via l’Unit of Work.

        public async Task<Ingredient?> GetIngredientByIdAsync(int id)
            // Récupère un ingrédient grâce à son identifiant.
            // Peut renvoyer null si aucun ingrédient ne correspond.
            => await _uow.Ingredients.GetAsync(id);
        // Utilise le repository Ingredients pour la récupération.

        public async Task<Ingredient> AddIngredientAsync(Ingredient newIngredient)
            // Ajoute un nouvel ingrédient en base.
            // Retourne l’ingrédient créé (avec Id, etc.).
            => await _uow.Ingredients.CreateAsync(newIngredient);
        // Délègue la création au repository.

        public async Task<Ingredient> ModifyIngredientAsync(Ingredient updateIngredient)
            // Met à jour un ingrédient existant.
            // Retourne l’ingrédient mis à jour.
            => await _uow.Ingredients.ModifyAsync(updateIngredient);
        // Appel au repository pour effectuer la modification.

        public async Task<bool> DeleteIngredientAsync(int id)
            // Supprime un ingrédient via son identifiant.
            // Retourne true en cas de succès.
            => await _uow.Ingredients.DeleteAsync(id);
        // Délègue la suppression au repository d’ingrédients.

        #endregion

        #region Gestion des relations Recette ↔ Ingrédient
        // Région dédiée à la gestion des relations many-to-many entre Recette et Ingredient.

        public async Task<bool> AddRecetteIngredientRelationshipAsync(int idIngredient, int idRecette)
            // Crée une relation entre une recette et un ingrédient (ajout dans la table de liaison).
            // Retourne true si l’opération réussit.
            => await _uow.Recettes.AddRecetteIngredientRelationshipAsync(idIngredient, idRecette);
        // Délègue au repository Recettes la création de la relation.

        public async Task<bool> RemoveRecetteIngredientRelationshipAsync(int idIngredient, int idRecette)
            // Supprime la relation entre une recette et un ingrédient.
            // Retourne true si la relation a bien été retirée.
            => await _uow.Recettes.RemoveRecetteIngredientRelationshipAsync(idIngredient, idRecette);
        // Utilise le repository Recettes pour supprimer la liaison.

        public async Task<IEnumerable<Recette>> GetRecettesByIdIngredientAsync(int idIngredient)
            // Récupère toutes les recettes qui utilisent un ingrédient donné (via la relation).
            => await _uow.Recettes.GetRecettesByIdIngredientAsync(idIngredient);
        // Appel au repository Recettes pour obtenir ces données.

        public async Task<IEnumerable<Ingredient>> GetIngredientsByIdRecetteAsync(int idRecette)
            // Selon ton DAL, cette méthode peut vivre côté Recettes ou Ingrédients.
            // Conserve celle qui existe réellement.
            // Récupère tous les ingrédients liés à une recette donnée.
            => await _uow.Ingredients.GetIngredientsByIdRecetteAsync(idRecette);
        // => await _uow.Recettes.GetIngredientsByIdRecetteAsync(idRecette);
        // Ligne commentée alternative, montrant qu’on pourrait aussi implémenter la méthode côté repository Recettes.

        public async Task<bool> DeleteRecetteRelationsAsync(int idRecette)
            // Supprime toutes les relations entre une recette donnée et ses ingrédients associés.
            // Pratique avant de supprimer la recette elle-même.
            => await _uow.Recettes.DeleteRecetteRelationsAsync(idRecette);
        // Délègue la suppression des relations au repository Recettes.

        public async Task<bool> DeleteIngredientRelationsAsync(int idIngredient)
            // Supprime toutes les relations entre un ingrédient donné et les recettes qui l’utilisent.
            => await _uow.Ingredients.DeleteIngredientRelationsAsync(idIngredient);
        // Délègue au repository Ingredients la suppression des liaisons.

        #endregion
    }
}
