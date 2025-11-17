using Dapper;
using SaveursInedites_Jalon2.DataAccessLayer.Session;
using SaveursInedites_Jalon2.Domain;

using SaveursInedites_Jalon2.Domain.BO;

namespace SaveursInedites_Jalon2.DataAccessLayer.Repositories.Utilisateurs
{
    public class UtilisateurRepository : IUtilisateurRepository
    {
        const string UTILISATEUR_TABLE = "utilisateur";
      
        readonly IDBSession _dbSession;

        public UtilisateurRepository(IDBSession dbSession)
        {
            _dbSession = dbSession;
        }

        public async Task<IEnumerable<Utilisateur>> GetAllAsync()
        {
            string query = $"SELECT * FROM {UTILISATEUR_TABLE}";
            return await _dbSession.Connection.QueryAsync<Utilisateur>(query);
        }

        public async Task<Utilisateur> GetAsync(int id)
        {
            string query = $"SELECT * FROM {UTILISATEUR_TABLE} WHERE id = @id";
            return await _dbSession.Connection.QuerySingleOrDefaultAsync<Utilisateur>(query, new { id });
        }

        public async Task<Utilisateur> CreateAsync(Utilisateur utilisateur)
        {
            string query = string.Empty;

            if (_dbSession.DatabaseProviderName == DatabaseProviderName.MariaDB || _dbSession.DatabaseProviderName == DatabaseProviderName.MySQL)
                query = $"INSERT INTO {UTILISATEUR_TABLE}(identifiant, email, password, role_id) VALUES(@Identifiant, @Email, @Password, @Role_id); Select LAST_INSERT_ID()";
            else if (_dbSession.DatabaseProviderName == DatabaseProviderName.PostgreSQL)
                query = $"INSERT INTO {UTILISATEUR_TABLE}(identifiant, email, password, role_id) VALUES(@Identifiant, @Email, @Password, @Role_id) RETURNING id";

            int lastId = await _dbSession.Connection.ExecuteScalarAsync<int>(query, utilisateur);
            utilisateur.Id = lastId;
            return utilisateur;
        }

        public async Task<Utilisateur> ModifyAsync(Utilisateur utilisateur)
        {
            string query = $"UPDATE {UTILISATEUR_TABLE} SET identifiant = @Identifiant, email = @Email, password = @Password WHERE id = @Id";
            int numLine = await _dbSession.Connection.ExecuteAsync(query, utilisateur);
            return numLine == 0 ? null : utilisateur;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            string query = $"DELETE FROM {UTILISATEUR_TABLE} WHERE id = @id";
            int numLine = await _dbSession.Connection.ExecuteAsync(query, new { id });
            return numLine != 0;
        }

        public async Task<Utilisateur?> GetByIdentifiantAsync(string identifiant)
        {
            string query = $"SELECT * FROM {UTILISATEUR_TABLE} WHERE identifiant = @identifiant";
            return await _dbSession.Connection.QuerySingleOrDefaultAsync<Utilisateur>(query, new { identifiant });
        }



        #region Methods specific to UtilisateurRepository
        public async Task<IEnumerable<Utilisateur>> GetUtilisateursByIdRecetteAsync(int idRecette)
        {
            string query = $"SELECT u.* FROM {UTILISATEUR_TABLE} u JOIN recettes r ON u.id = r.createur WHERE r.id = @idRecette";
            return await _dbSession.Connection.QueryAsync<Utilisateur>(query, new { idRecette }, _dbSession.Transaction);
        }

        public Task<bool> DeleteUtilisateurRelationsAsync(int idUtilisateur)
        {
            throw new NotImplementedException();
        }

        #endregion Methods specific to UtilisateurRepository
    }
}
