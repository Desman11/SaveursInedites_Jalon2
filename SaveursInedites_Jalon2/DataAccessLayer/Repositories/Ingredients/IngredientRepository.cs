using Dapper;
using SaveursInedites_Jalon2.DataAccessLayer.Session;
using SaveursInedites_Jalon2.Domain;
using SaveursInedites_Jalon2.Domain.BO;

namespace SaveursInedites_Jalon2.DataAccessLayer.Repositories.Ingredients
{

    public class IngredientRepository : IIngredientRepository
    {
        const string INGREDIENT_TABLE = "ingredient";
        const string RECETTE_INGREDIENT_TABLE = "recette_ingredient";

        readonly IDBSession _dbSession;

        public IngredientRepository(IDBSession dbSession)
        {
            _dbSession = dbSession;
        }

        public async Task<IEnumerable<Ingredient>> GetAllAsync()
        {
            string query = $"SELECT * FROM {INGREDIENT_TABLE}";
            return await _dbSession.Connection.QueryAsync<Ingredient>(query, transaction: _dbSession.Transaction);
        }

        public async Task<Ingredient> GetAsync(int id)
        {
            string query = $"SELECT * FROM {INGREDIENT_TABLE} WHERE id = @id";
            return await _dbSession.Connection.QuerySingleOrDefaultAsync<Ingredient>(query, new { id }, transaction: _dbSession.Transaction);
        }

        public async Task<Ingredient> CreateAsync(Ingredient ingredient)
        {
            string query = string.Empty;

            if (_dbSession.DatabaseProviderName == DatabaseProviderName.MariaDB || _dbSession.DatabaseProviderName == DatabaseProviderName.MySQL)
            {
                query = $@"
                    INSERT INTO {INGREDIENT_TABLE}(nom) 
                    VALUES(@Nom); 
                    SELECT LAST_INSERT_ID();";
            }
            else if (_dbSession.DatabaseProviderName == DatabaseProviderName.PostgreSQL)
            {
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
        {
            string query = $"UPDATE {INGREDIENT_TABLE} SET nom = @nom WHERE id = @Id";
            int numLine = await _dbSession.Connection.ExecuteAsync(query, ingredient, transaction: _dbSession.Transaction);
            return numLine == 0 ? null : ingredient;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            string query = $"DELETE FROM {INGREDIENT_TABLE} WHERE id = @id";
            int numLine = await _dbSession.Connection.ExecuteAsync(query, new { id }, transaction: _dbSession.Transaction);
            return numLine != 0;
        }
        #region Methods specific to IngredientRepository

        public async Task<bool> AddIngredientRecetteRelationshipAsync(int idRecette, int idIngredient)
        {
            string query = $"INSERT INTO {RECETTE_INGREDIENT_TABLE}(idRecette, idIngredient) VALUES(@idRecette, @idIngredient)";
            int numLine = await _dbSession.Connection.ExecuteAsync(query, new { idRecette, idIngredient }, transaction: _dbSession.Transaction);
            return numLine != 0;
        }

        public async Task<bool> RemoveIngredientRecetteRelationshipAsync(int idRecette, int idIngredient)
        {
            string query = $"DELETE FROM {RECETTE_INGREDIENT_TABLE} WHERE idRecette = @idRecette AND idIngredient = @idIngredient";
            int numLine = await _dbSession.Connection.ExecuteAsync(query, new { idRecette, idIngredient }, transaction: _dbSession.Transaction);
            return numLine != 0;
        }

        public async Task<IEnumerable<Ingredient>> GetIngredientsByIdRecetteAsync(int idRecette)
        {
            string query = $"SELECT b.* FROM {INGREDIENT_TABLE} b JOIN {RECETTE_INGREDIENT_TABLE} ab ON b.id = ab.idIngredient WHERE ab.idRecette = @idRecette";
            return await _dbSession.Connection.QueryAsync<Ingredient>(query, new { idRecette }, transaction: _dbSession.Transaction);
        }

        public async Task<bool> DeleteIngredientRelationsAsync(int idIngredient)
        {
            string query = $"DELETE FROM {RECETTE_INGREDIENT_TABLE} WHERE idIngredient = @idIngredient";
            int numLine = await _dbSession.Connection.ExecuteAsync(query, new { idIngredient }, transaction: _dbSession.Transaction);
            return numLine != 0;
        }

        #endregion Methods specific to IngredientRepository
    }
}
