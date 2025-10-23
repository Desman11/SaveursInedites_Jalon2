using Dapper;
using SaveursInedites_Jalon2.DataAccessLayer.Session;
using SaveursInedites_Jalon2.Domain;
using SaveursInedites_Jalon2.Domain.BO;

namespace SaveursInedites_Jalon2.DataAccessLayer.Repositories.Recettes
{

    public class RecetteRepository : IRecetteRepository
    {
        const string RECETTE_TABLE = "recette";
        const string UTILISATEUR_RECETTE_TABLE = "utilisateur_recette";
        
        readonly IDBSession _dbSession;

        public RecetteRepository(IDBSession dbSession)
        {
            _dbSession = dbSession;
        }

        public async Task<IEnumerable<Recette>> GetAllAsync()
        {
            string query = $"SELECT * FROM {RECETTE_TABLE}";
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
        #region Methods specific to RecetteRepository

        public async Task<bool> AddRecetteUtilisateurRelationshipAsync(int idUtilisateur, int idRecette)
        {
            string query = $"INSERT INTO {UTILISATEUR_RECETTE_TABLE}(idutilisateur, idrecette) VALUES(@idUtilisateur, @idRecette)";
            int numLine = await _dbSession.Connection.ExecuteAsync(query, new { idUtilisateur, idRecette }, transaction: _dbSession.Transaction);
            return numLine != 0;
        }

        public async Task<bool> RemoveRecetteUtilisateurRelationshipAsync(int idUtilisateur, int idRecette)
        {
            string query = $"DELETE FROM {UTILISATEUR_RECETTE_TABLE} WHERE idutilisateur = @idUtilisateur AND idrecette = @idRecette";
            int numLine = await _dbSession.Connection.ExecuteAsync(query, new { idUtilisateur, idRecette }, transaction: _dbSession.Transaction);
            return numLine != 0;
        }

        public async Task<IEnumerable<Recette>> GetRecettesByIdUtilisateurAsync(int idUtilisateur)
        {
            string query = $"SELECT b.* FROM {RECETTE_TABLE} b JOIN {UTILISATEUR_RECETTE_TABLE} ab ON b.id = ab.idRecette WHERE ab.idutilisateur = @idUtilisateur";
            return await _dbSession.Connection.QueryAsync<Recette>(query, new { idUtilisateur }, transaction: _dbSession.Transaction);
        }

        public async Task<bool> DeleteRecetteRelationsAsync(int idRecette)
        {
            string query = $"DELETE FROM {UTILISATEUR_RECETTE_TABLE} WHERE idrecette = @idRecette";
            int numLine = await _dbSession.Connection.ExecuteAsync(query, new { idRecette }, transaction: _dbSession.Transaction);
            return numLine != 0;
        }

        public Task<bool> AddRecetteIngredientRelationshipAsync(int idIngredient, int idRecette)
        {
            throw new NotImplementedException();
        }

        public Task<bool> RemoveRecetteIngredientRelationshipAsync(int idIngredient, int idRecette)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Recette>> GetRecettesByIdIngredientAsync(int idIngredient)
        {
            throw new NotImplementedException();
        }

        #endregion Methods specific to RecetteRepository
    }
}
    

