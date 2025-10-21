using Dapper;
using SaveursInedites_Jalon2.DataAccessLayer.Session;
using SaveursInedites_Jalon2.Domain;
using SaveursInedites_Jalon2.Domain.BO;

namespace SaveursInedites_Jalon2.DataAccessLayer.Repositories.Ingredients
{
    public class IngredientRepository : IIngredientRepository
    {
        const string INGREDIENT_TABLE = "ingredient";
        const string INGREDIENT_RECETTE_TABLE = "ingredient_recette";
        const string RECETTE_TABLE = "recette";
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

        public async Task<Ingredient> CreateAsync(Ingredient ingredients)
        {
            string query = string.Empty;

            if (_dbSession.DatabaseProviderName == DatabaseProviderName.MariaDB || _dbSession.DatabaseProviderName == DatabaseProviderName.MySQL)
                query = $"INSERT INTO {INGREDIENT_TABLE}(name, quantity) VALUES(@Name, @Quantity); Select LAST_INSERT_ID()";
            else if (_dbSession.DatabaseProviderName == DatabaseProviderName.PostgreSQL)
                query = $"INSERT INTO {INGREDIENT_TABLE}(name, quantity) VALUES(@Name, @Quantity) RETURNING id";

            int lastId = await _dbSession.Connection.ExecuteScalarAsync<int>(query, ingredients, transaction: _dbSession.Transaction);
            ingredients.Id = lastId;
            return ingredients;
        }

        public async Task<Ingredient> ModifyAsync(Ingredient ingredients)
        {
            string query = $"UPDATE {INGREDIENT_TABLE} SET name = @Name, quantity = @Quantity WHERE id = @Id";
            int numLine = await _dbSession.Connection.ExecuteAsync(query, ingredients, transaction: _dbSession.Transaction);
            return numLine == 0 ? null : ingredients;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            string query = $"DELETE FROM {INGREDIENT_TABLE} WHERE id = @id";
            int numLine = await _dbSession.Connection.ExecuteAsync(query, new { id }, transaction: _dbSession.Transaction);
            return numLine != 0;
        }

        #region Methods specific to IngredientRepository   

       
        public async Task<bool> AddRecetteIngredientRelationshipAsync(int idIngredient, int idRecette)
        {
            string query = $"INSERT INTO {INGREDIENT_RECETTE_TABLE}(idingredient, idrecette) VALUES(@idIngredient, @idRecette)";
            int numLine = await _dbSession.Connection.ExecuteAsync(query, new { idIngredient, idRecette }, transaction: _dbSession.Transaction);
            return numLine != 0;
        }

        public async Task<bool> RemoveRecetteIngredientRelationshipAsync(int idIngredient, int idRecette)
        {
            string query = $"DELETE FROM {INGREDIENT_RECETTE_TABLE} WHERE idingredient = @idIngredient AND idrecette = @idRecette";
            int numLine = await _dbSession.Connection.ExecuteAsync(query, new { idIngredient, idRecette }, transaction: _dbSession.Transaction);
            return numLine != 0;
        }

        public async Task<IEnumerable<Recette>> GetRecettesByIdIngredientAsync(int idIngredient)
        {
            string query = $"SELECT b.* FROM {RECETTE_TABLE} b JOIN {INGREDIENT_RECETTE_TABLE} ab ON b.id = ab.idrecette WHERE ab.idingredient = @idIngredient";
            return await _dbSession.Connection.QueryAsync<Recette>(query, new { idIngredient }, transaction: _dbSession.Transaction);
        }

        public async Task<bool> DeleteRecetteRelationsAsync(int idRecette)
        {
            string query = $"DELETE FROM {INGREDIENT_RECETTE_TABLE} WHERE idrecette = @idRecette";
            int numLine = await _dbSession.Connection.ExecuteAsync(query, new { idRecette }, transaction: _dbSession.Transaction);
            return numLine != 0;
        }

        #endregion Methods specific to IngredientRepository
    }
}

