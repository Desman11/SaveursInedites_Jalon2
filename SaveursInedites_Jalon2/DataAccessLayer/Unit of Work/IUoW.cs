using SaveursInedites_Jalon2.DataAccessLayer.Repositories.Recettes;
using SaveursInedites_Jalon2.DataAccessLayer.Repositories.Utilisateurs;
using SaveursInedites_Jalon2.DataAccessLayer.Repositories.Ingredients;
// Imports des interfaces de repositories nécessaires pour l’Unit of Work.

namespace SaveursInedites_Jalon2.DataAccessLayer.Unit_of_Work
// Namespace regroupant les éléments liés au pattern Unit of Work (UoW).
{
    public interface IUoW
    // Interface définissant le contrat de l’Unit of Work,
    // qui centralise les repositories et la gestion des transactions.
    {
        #region Repositories

        IRecetteRepository Recettes { get; }
        // Accès au repository des recettes.

        IUtilisateurRepository Utilisateurs { get; }
        // Accès au repository des utilisateurs.

        IIngredientRepository Ingredients { get; }
        // Accès au repository des ingrédients.

        #endregion

        #region Transactions

        bool HasActiveTransaction { get; }
        // Indique si une transaction est actuellement ouverte sur la session BDD.

        void BeginTransaction();
        // Démarre une transaction en base.

        void Commit();
        // Valide toutes les opérations effectuées durant la transaction.

        void Rollback();
        // Annule les opérations si une erreur survient.

        #endregion Transactions
    }
}
