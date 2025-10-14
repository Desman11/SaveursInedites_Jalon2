using Dapper;
using SaveursInedites_Jalon2.DataAccessLayer.Session;
using SaveursInedites_Jalon2.Domain;
using SaveursInedites_Jalon2.Domain.BO;

namespace SaveursInedites_Jalon2.DataAccessLayer.Repositories.Recettes
{

    public class RecetteRepository : IRecetteRepository
    {
        const string RECETTE_TABLE = "recette";
        readonly IDBSession _dbSession;

        public RecetteRepository(IDBSession dbSession)
        {
            _dbSession = dbSession;
        }

        public async Task<IEnumerable<Recette>> GetAllAsync()
        {
            string query = "SELECT * FROM Recette";
            return await _dbSession.Connection.QueryAsync<Recette>(query, transaction: _dbSession.Transaction);
        }

        public async Task<Recette> GetAsync(int id)
        {
            string query = $"SELECT * FROM {RECETTE_TABLE} WHERE id = @id";
            return await _dbSession.Connection.QuerySingleOrDefaultAsync<Recette>(query, new { id }, transaction: _dbSession.Transaction);
        }

        public async Task<Recette> CreateAsync(Recette recette)
        {
            string query = string.Empty;

            if (_dbSession.DatabaseProviderName == DatabaseProviderName.MariaDB || _dbSession.DatabaseProviderName == DatabaseProviderName.MySQL)
            {
                query = $@"
                    INSERT INTO {RECETTE_TABLE}(nom, temps_preparation, temps_cuisson, difficulte, photo, createur) 
                    VALUES(@Nom, @TempsPreparation, @TempsCuisson, @Difficulte, @Photo, @Createur); 
                    SELECT LAST_INSERT_ID();";
            }
            else if (_dbSession.DatabaseProviderName == DatabaseProviderName.PostgreSQL)
            {
                query = $@"
                    INSERT INTO {RECETTE_TABLE}(nom, temps_preparation, temps_cuisson, difficulte, photo, createur) 
                    VALUES(@Nom, @TempsPreparation, @TempsCuisson, @Difficulte, @Photo, @Createur) 
                    RETURNING id;";
            }

            int lastId = _dbSession.Connection.ExecuteScalar<int>(query, recette, transaction: _dbSession.Transaction);
            recette.Id = lastId;
            return recette;
        }

        public async Task<Recette> ModifyAsync(Recette recette)
        {
            string query = $@"
                UPDATE {RECETTE_TABLE} 
                SET nom = @Nom, 
                    temps_preparation = @TempsPreparation, 
                    temps_cuisson = @TempsCuisson, 
                    difficulte = @Difficulte, 
                    photo = @Photo, 
                    createur = @Createur
                WHERE id = @Id";

            int numLine = await _dbSession.Connection.ExecuteAsync(query, recette, transaction: _dbSession.Transaction);
            return numLine == 0 ? null : recette;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            string query = $"DELETE FROM {RECETTE_TABLE} WHERE id = @id";
            int numLine = await _dbSession.Connection.ExecuteAsync(query, new { id }, transaction: _dbSession.Transaction);
            return numLine != 0;
        }

    }
}
