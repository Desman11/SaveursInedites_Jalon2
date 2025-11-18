using Dapper;
// Importe Dapper, micro-ORM utilisé pour exécuter des requêtes SQL et mapper les résultats sur des objets C#.

using SaveursInedites_Jalon2.DataAccessLayer.Session;
// Permet d'accéder à l’interface IDBSession, qui gère la connexion et la transaction.

using SaveursInedites_Jalon2.Domain;
// Contient notamment l’énumération DatabaseProviderName.

using SaveursInedites_Jalon2.Domain.BO;
// Contient la classe métier Utilisateur.

namespace SaveursInedites_Jalon2.DataAccessLayer.Repositories.Utilisateurs
// Namespace pour les repositories liés aux utilisateurs.
{
    public class UtilisateurRepository : IUtilisateurRepository
    // Implémentation concrète du repository d’utilisateurs, basée sur Dapper.
    {
        const string UTILISATEUR_TABLE = "utilisateur";
        // Nom de la table en base de données. Centralisé pour éviter les répétitions littérales.

        readonly IDBSession _dbSession;
        // Session de base de données injectée (connexion + transaction éventuelle).

        public UtilisateurRepository(IDBSession dbSession)
        // Constructeur recevant la session BDD via l’injection de dépendances.
        {
            _dbSession = dbSession;
            // Stocke la session pour réutilisation dans les méthodes.
        }

        public async Task<IEnumerable<Utilisateur>> GetAllAsync()
        // Récupère tous les utilisateurs dans la table.
        {
            string query = $"SELECT * FROM {UTILISATEUR_TABLE}";
            // Requête SQL sélectionnant toutes les lignes de la table utilisateur.

            return await _dbSession.Connection.QueryAsync<Utilisateur>(query);
            // Exécution de la requête avec Dapper et mapping vers une collection d’Utilisateur.
        }

        public async Task<Utilisateur> GetAsync(int id)
        // Récupère un utilisateur par son identifiant.
        {
            string query = $"SELECT * FROM {UTILISATEUR_TABLE} WHERE id = @id";
            // Requête paramétrée filtrant sur la colonne id.

            return await _dbSession.Connection.QuerySingleOrDefaultAsync<Utilisateur>(query, new { id });
            // Retourne soit l’utilisateur correspondant, soit null si aucun résultat.
        }

        public async Task<Utilisateur> CreateAsync(Utilisateur utilisateur)
        // Crée un nouvel utilisateur en base, puis retourne l’objet avec son Id renseigné.
        {
            string query = string.Empty;
            // Chaîne de requête initialisée vide, remplie selon le provider BDD.

            if (_dbSession.DatabaseProviderName == DatabaseProviderName.MariaDB || _dbSession.DatabaseProviderName == DatabaseProviderName.MySQL)
                // Cas MariaDB ou MySQL : syntaxe spécifique pour récupérer le dernier Id inséré.
                query = $"INSERT INTO {UTILISATEUR_TABLE}(identifiant, email, password, role_id) VALUES(@Identifiant, @Email, @Password, @Role_id); Select LAST_INSERT_ID()";
            else if (_dbSession.DatabaseProviderName == DatabaseProviderName.PostgreSQL)
                // Cas PostgreSQL : on utilise RETURNING id pour récupérer la clé primaire.
                query = $"INSERT INTO {UTILISATEUR_TABLE}(identifiant, email, password, role_id) VALUES(@Identifiant, @Email, @Password, @Role_id) RETURNING id";

            int lastId = await _dbSession.Connection.ExecuteScalarAsync<int>(query, utilisateur);
            // Exécute la requête d’insertion et récupère l’Id généré.

            utilisateur.Id = lastId;
            // Affecte l’Id généré à l’objet Utilisateur en mémoire.

            return utilisateur;
            // Retourne l’utilisateur nouvellement créé avec son Id.
        }

        public async Task<Utilisateur> ModifyAsync(Utilisateur utilisateur)
        // Met à jour les champs identifiant, email et password pour un utilisateur donné.
        {
            string query = $"UPDATE {UTILISATEUR_TABLE} SET identifiant = @Identifiant, email = @Email, password = @Password WHERE id = @Id";
            // Requête d’update paramétrée basée sur l’Id.

            int numLine = await _dbSession.Connection.ExecuteAsync(query, utilisateur);
            // Exécute la requête de mise à jour et retourne le nombre de lignes affectées.

            return numLine == 0 ? null : utilisateur;
            // Si aucune ligne n’est modifiée, renvoie null (aucun utilisateur trouvé),
            // sinon renvoie l’objet utilisateur mis à jour.
            // (Note : la signature ne prévoit pas de type nullable, mais le code renvoie null.)
        }

        public async Task<bool> DeleteAsync(int id)
        // Supprime un utilisateur par son Id. Retourne true si au moins une ligne est supprimée.
        {
            string query = $"DELETE FROM {UTILISATEUR_TABLE} WHERE id = @id";
            // Requête de suppression paramétrée.

            int numLine = await _dbSession.Connection.ExecuteAsync(query, new { id });
            // Exécute la requête et récupère le nombre de lignes supprimées.

            return numLine != 0;
            // Retourne true si une (ou plusieurs) lignes ont été affectées.
        }

        public async Task<Utilisateur?> GetByIdentifiantAsync(string identifiant)
        // Récupère un utilisateur à partir de son identifiant (login).
        {
            string query = $"SELECT * FROM {UTILISATEUR_TABLE} WHERE identifiant = @identifiant";
            // Requête SQL paramétrée sur la colonne identifiant.

            return await _dbSession.Connection.QuerySingleOrDefaultAsync<Utilisateur>(query, new { identifiant });
            // Retourne l’utilisateur s’il existe, sinon null.
        }

        #region Methods specific to UtilisateurRepository

        public async Task<IEnumerable<Utilisateur>> GetUtilisateursByIdRecetteAsync(int idRecette)
        // Récupère les utilisateurs liés à une recette (ici, le créateur de la recette).
        {
            string query = $"SELECT u.* FROM {UTILISATEUR_TABLE} u JOIN recettes r ON u.id = r.createur WHERE r.id = @idRecette";
            // Jointure entre la table utilisateur et recettes sur la clé createur, filtrée par l’id de la recette.

            return await _dbSession.Connection.QueryAsync<Utilisateur>(query, new { idRecette }, _dbSession.Transaction);
            // Exécute la requête dans le contexte de la transaction courante et mappe les résultats vers Utilisateur.
        }

        public Task<bool> DeleteUtilisateurRelationsAsync(int idUtilisateur)
        // Méthode prévue pour supprimer les relations associées à un utilisateur (ex. liens avec d’autres tables).
        {
            throw new NotImplementedException();
            // Actuellement non implémentée : l’appel de cette méthode provoquera une exception.
        }

        #endregion Methods specific to UtilisateurRepository
    }
}
