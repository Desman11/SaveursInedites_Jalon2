using Dapper;
using SaveursInedites_Jalon2.DataAccessLayer.Session;
using SaveursInedites_Jalon2.Domain;
using SaveursInedites_Jalon2.Domain.BO;

namespace SaveursInedites_Jalon2.DataAccessLayer.Repositories.Ingredients
{
    public class IngredientRepository : IIngredientRepository
    {
        const string INGREDIENT_TABLE = "Ingredient";
        const string RECETTE_INGREDIENT_TABLE = "recette_Ingredient";
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

        public async Task<Ingredient> CreateAsync(Ingredient Ingredient)
        {
            string query = string.Empty;

            if (_dbSession.DatabaseProviderName == DatabaseProviderName.MariaDB || _dbSession.DatabaseProviderName == DatabaseProviderName.MySQL)
                query = $"INSERT INTO {INGREDIENT_TABLE}(nom) VALUES(@Nom); Select LAST_INSERT_ID()";
            else if (_dbSession.DatabaseProviderName == DatabaseProviderName.PostgreSQL)
                query = $"INSERT INTO {INGREDIENT_TABLE}(nom) VALUES(@Nom) RETURNING id";

            int lastId = await _dbSession.Connection.ExecuteScalarAsync<int>(query, Ingredient, transaction: _dbSession.Transaction);
            Ingredient.Id = lastId;
            return Ingredient;
        }

        public async Task<Ingredient> ModifyAsync(Ingredient Ingredient)
        {
            string query = $"UPDATE {INGREDIENT_TABLE} SET nom = @Nom WHERE id = @Id";
            int numLine = await _dbSession.Connection.ExecuteAsync(query, Ingredient, transaction: _dbSession.Transaction);
            return numLine == 0 ? null : Ingredient;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            string query = $"DELETE FROM {INGREDIENT_TABLE} WHERE id = @id";
            int numLine = await _dbSession.Connection.ExecuteAsync(query, new { id }, transaction: _dbSession.Transaction);
            return numLine != 0;
        }

        #region Methods specific to IngredientRepository

        public async Task<bool> AddIngredientrecetteRelationshipAsync(int idrecette, int idingredient)
        {
            string query = $"INSERT INTO {RECETTE_INGREDIENT_TABLE}(idrecette, idingredient) VALUES(@idRecette, @idIngredient)";
            int numLine = await _dbSession.Connection.ExecuteAsync(query, new { idrecette, idingredient }, transaction: _dbSession.Transaction);
            return numLine != 0;
        }

        public async Task<bool> RemoveIngredientrecetteRelationshipAsync(int idrecette, int idingredient)
        {
            string query = $"DELETE FROM {RECETTE_INGREDIENT_TABLE} WHERE idrecette = @idRecette AND idingredient = @idIngredient";
            int numLine = await _dbSession.Connection.ExecuteAsync(query, new { idrecette, idingredient }, transaction: _dbSession.Transaction);
            return numLine != 0;
        }

        public async Task<IEnumerable<Ingredient>> GetIngredientsByIdrecetteAsync(int idrecette)
        {
            string query = $"SELECT b.* FROM {INGREDIENT_TABLE} b JOIN {RECETTE_INGREDIENT_TABLE} ab ON b.id = ab.idingredient WHERE ab.idrecette = @idrecette";
            return await _dbSession.Connection.QueryAsync<Ingredient>(query, new { idrecette }, transaction: _dbSession.Transaction);
        }

        public async Task<bool> DeleteIngredientRelationsAsync(int idingredient)
        {
            string query = $"DELETE FROM {RECETTE_INGREDIENT_TABLE} WHERE idingredient = @idIngredient";
            int numLine = await _dbSession.Connection.ExecuteAsync(query, new { idingredient }, transaction: _dbSession.Transaction);
            return numLine != 0;
        }

        Task<bool> IIngredientRepository.AddIngredientRecetteRelationshipAsync(int idRecette, int idIngredient)
        {
            throw new NotImplementedException();
        }

        Task<bool> IIngredientRepository.RemoveIngredientRecetteRelationshipAsync(int idRecette, int idIngredient)
        {
            throw new NotImplementedException();
        }

        Task<IEnumerable<Ingredient>> IIngredientRepository.GetIngredientsByIdRecetteAsync(int idRecette)
        {
            throw new NotImplementedException();
        }

        Task<bool> IIngredientRepository.DeleteIngredientRelationsAsync(int idIngredient)
        {
            throw new NotImplementedException();
        }

        Task<IEnumerable<Ingredient>> IGenericReadRepository<int, Ingredient>.GetAllAsync()
        {
            throw new NotImplementedException();
        }

        Task<Ingredient> IGenericReadRepository<int, Ingredient>.GetAsync(int key)
        {
            throw new NotImplementedException();
        }

        Task<Ingredient> IGenericWriteRepository<int, Ingredient>.CreateAsync(Ingredient entity)
        {
            throw new NotImplementedException();
        }

        Task<Ingredient> IGenericWriteRepository<int, Ingredient>.ModifyAsync(Ingredient entity)
        {
            throw new NotImplementedException();
        }

        Task<bool> IGenericWriteRepository<int, Ingredient>.DeleteAsync(int key)
        {
            throw new NotImplementedException();
        }

        #endregion Methods specific to IngredientRepository
    }
}
