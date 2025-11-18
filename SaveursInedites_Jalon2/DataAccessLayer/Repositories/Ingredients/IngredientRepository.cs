using Dapper;
// Micro-ORM utilisé pour exécuter des requêtes SQL et mapper les résultats sur des objets C#.

using SaveursInedites_Jalon2.DataAccessLayer.Session;
// Accès à IDBSession : connexion + transaction.

using SaveursInedites_Jalon2.Domain;
// Contient DatabaseProviderName pour adapter les requêtes selon le SGBD.

using SaveursInedites_Jalon2.Domain.BO;
// Modèle métier Ingredient.

namespace SaveursInedites_Jalon2.DataAccessLayer.Repositories.Ingredients
// Namespace du repository Ingredient.
{
    public class IngredientRepository : IIngredientRepository
    // Implémentation du repository des ingrédients.
    {
        const string INGREDIENT_TABLE = "ingredient";
        // Nom de la table des ingrédients.

        const string RECETTE_INGREDIENT_TABLE = "recette_ingredient";
        // Nom de la table de liaison recette ↔ ingrédient.

        readonly IDBSession _dbSession;
        // Session BD injectée (connexion + transaction partagée).

        public IngredientRepository(IDBSession dbSession)
        // Constructeur : reçoit la session via DI.
        {
            _dbSession = dbSession;
        }

        public async Task<IEnumerable<Ingredient>> GetAllAsync()
        // Récupère tous les ingrédients.
        {
            string query = $"SELECT * FROM {INGREDIENT_TABLE}";
            return await _dbSession.Connection.QueryAsync<Ingredient>(query, transaction: _dbSession.Transaction);
        }

        public async Task<Ingredient> GetAsync(int id)
        // Récupère un ingrédient par son identifiant.
        {
            string query = $"SELECT * FROM {INGREDIENT_TABLE} WHERE id = @id";
            return await _dbSession.Connection.QuerySingleOrDefaultAsync<Ingredient>(query, new { id }, transaction: _dbSession.Transaction);
        }

        public async Task<Ingredient> CreateAsync(Ingredient ingredient)
        // Insère un ingrédient et retourne l’objet avec son Id renseigné.
        {
            string query = string.Empty;

            if (_dbSession.DatabaseProviderName == DatabaseProviderName.MariaDB ||
                _dbSession.DatabaseProviderName == DatabaseProviderName.MySQL)
            {
                // Syntaxe MySQL/MariaDB : LAST_INSERT_ID()
                query = $@"
                    INSERT INTO {INGREDIENT_TABLE}(nom) 
                    VALUES(@Nom); 
                    SELECT LAST_INSERT_ID();";
            }
            else if (_dbSession.DatabaseProviderName == DatabaseProviderName.PostgreSQL)
            {
                // Syntaxe PostgreSQL : RETURNING id
                query = $@"
                    INSERT INTO {INGREDIENT_TABLE}(nom) 
                    VALUES(@Nom) 
                    RETURNING id;";
            }

            int lastId = _dbSession.Connection.ExecuteScalar<int>(query, ingredient, transaction: _dbSession.Transaction);
            ingredient.Id = lastId;
            return ingredient;
        }

        public async Task<Ingredient> ModifyAsync(Ingredient ingredient)
        // Met à jour un ingrédient existant.
        {
            string query = $"UPDATE {INGREDIENT_TABLE} SET nom = @nom WHERE id = @Id";
            int numLine = await _dbSession.Connection.ExecuteAsync(query, ingredient, transaction: _dbSession.Transaction);
            return numLine == 0 ? null : ingredient;
        }

        public async Task<bool> DeleteAsync(int id)
        // Supprime un ingrédient par identifiant.
        {
            string query = $"DELETE FROM {INGREDIENT_TABLE} WHERE id = @id";
            int numLine = await _dbSession.Connection.ExecuteAsync(query, new { id }, transaction: _dbSession.Transaction);
            return numLine != 0;
        }

        #region Methods specific to IngredientRepository
        // Méthodes de gestion des relations recette ↔ ingrédient.

        public async Task<bool> AddIngredientRecetteRelationshipAsync(int idRecette, int idIngredient)
        // Ajoute une relation dans la table de liaison.
        {
            string query = $"INSERT INTO {RECETTE_INGREDIENT_TABLE}(idRecette, idIngredient) VALUES(@idRecette, @idIngredient)";
            int numLine = await _dbSession.Connection.ExecuteAsync(query, new { idRecette, idIngredient }, transaction: _dbSession.Transaction);
            return numLine != 0;
        }

        public async Task<bool> RemoveIngredientRecetteRelationshipAsync(int idRecette, int idIngredient)
        // Supprime une relation recette ↔ ingrédient.
        {
            string query = $"DELETE FROM {RECETTE_INGREDIENT_TABLE} WHERE idRecette = @idRecette AND idIngredient = @idIngredient";
            int numLine = await _dbSession.Connection.ExecuteAsync(query, new { idRecette, idIngredient }, transaction: _dbSession.Transaction);
            return numLine != 0;
        }

        public async Task<IEnumerable<Ingredient>> GetIngredientsByIdRecetteAsync(int idRecette)
        // Retourne tous les ingrédients associés à une recette donnée.
        {
            string query = $"SELECT b.* FROM {INGREDIENT_TABLE} b JOIN {RECETTE_INGREDIENT_TABLE} ab ON b.id = ab.idIngredient WHERE ab.idRecette = @idRecette";
            return await _dbSession.Connection.QueryAsync<Ingredient>(query, new { idRecette }, transaction: _dbSession.Transaction);
        }

        public async Task<bool> DeleteIngredientRelationsAsync(int idIngredient)
        // Supprime toutes les relations impliquant cet ingrédient.
        {
            string query = $"DELETE FROM {RECETTE_INGREDIENT_TABLE} WHERE idIngredient = @idIngredient";
            int numLine = await _dbSession.Connection.ExecuteAsync(query, new { idIngredient }, transaction: _dbSession.Transaction);
            return numLine != 0;
        }

        #endregion Methods specific to IngredientRepository
    }
}
