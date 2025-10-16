using Dapper;
using SaveursInedites_Jalon2.DataAccessLayer.Session;
using SaveursInedites_Jalon2.Domain;
using SaveursInedites_Jalon2.Domain.BO;

namespace SaveursInedites_Jalon2.DataAccessLayer.Repositories.Ingredients
{
    public class IngredientsRepository : IIngredientsRepository
    {
        const string INGREDIENTS_TABLE = "ingredients";
        readonly IDBSession _dbSession;

        public IngredientsRepository(IDBSession dbSession)
        {
            _dbSession = dbSession;
        }

        public async Task<IEnumerable<Ingredients>> GetAllAsync()
        {
            string query = $"SELECT * FROM {INGREDIENTS_TABLE}";
            return await _dbSession.Connection.QueryAsync<Ingredients>(query, transaction: _dbSession.Transaction);
        }

        public async Task<Ingredients> GetAsync(int id)
        {
            string query = $"SELECT * FROM {INGREDIENTS_TABLE} WHERE id = @id";
            return await _dbSession.Connection.QuerySingleOrDefaultAsync<Ingredients>(query, new { id }, transaction: _dbSession.Transaction);
        }

        public async Task<Ingredients> CreateAsync(Ingredients ingredients)
        {
            string query = string.Empty;

            if (_dbSession.DatabaseProviderName == DatabaseProviderName.MariaDB || _dbSession.DatabaseProviderName == DatabaseProviderName.MySQL)
                query = $"INSERT INTO {INGREDIENTS_TABLE}(name, quantity) VALUES(@Name, @Quantity); Select LAST_INSERT_ID()";
            else if (_dbSession.DatabaseProviderName == DatabaseProviderName.PostgreSQL)
                query = $"INSERT INTO {INGREDIENTS_TABLE}(name, quantity) VALUES(@Name, @Quantity) RETURNING id";

            int lastId = await _dbSession.Connection.ExecuteScalarAsync<int>(query, ingredients, transaction: _dbSession.Transaction);
            ingredients.Id = lastId;
            return ingredients;
        }

        public async Task<Ingredients> ModifyAsync(Ingredients ingredients)
        {
            string query = $"UPDATE {INGREDIENTS_TABLE} SET name = @Name, quantity = @Quantity WHERE id = @Id";
            int numLine = await _dbSession.Connection.ExecuteAsync(query, ingredients, transaction: _dbSession.Transaction);
            return numLine == 0 ? null : ingredients;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            string query = $"DELETE FROM {INGREDIENTS_TABLE} WHERE id = @id";
            int numLine = await _dbSession.Connection.ExecuteAsync(query, new { id }, transaction: _dbSession.Transaction);
            return numLine != 0;
        }

        #region Methods specific to IngredientsRepository   

        public async Task<IEnumerable<Ingredients>> GetIngredientsByIdRecipeAsync(int idRecipe)
        {
            string query = $"SELECT i.* FROM {INGREDIENTS_TABLE} i JOIN {RECIPE_INGREDIENTS_TABLE} ri ON i.id = ri.idingredient WHERE ri.idrecipe = @idRecipe";
            return await _dbSession.Connection.QueryAsync<Ingredients>(query, new { idRecipe }, transaction: _dbSession.Transaction);
        }

        public async Task<bool> DeleteIngredientRelationsAsync(int idIngredient)
        {
            string query = $"DELETE FROM {RECIPE_INGREDIENTS_TABLE} WHERE idingredient = @idIngredient";
            int numLine = await _dbSession.Connection.ExecuteAsync(query, new { idIngredient }, transaction: _dbSession.Transaction);
            return numLine != 0;
        }

        #endregion Methods specific to IngredientsRepository
    }
}

