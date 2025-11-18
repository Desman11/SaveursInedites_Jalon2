using Dapper;
// Importe Dapper, utilisé pour exécuter des requêtes SQL et mapper les résultats sur des objets C#.

using SaveursInedites_Jalon2.DataAccessLayer.Session;
// Permet d'accéder à IDBSession (connexion + transaction).

using SaveursInedites_Jalon2.Domain;
// Contient notamment l’énumération DatabaseProviderName.

using SaveursInedites_Jalon2.Domain.BO;
// Contient la classe métier Recette.

namespace SaveursInedites_Jalon2.DataAccessLayer.Repositories.Recettes
// Namespace des repositories liés aux recettes.
{
    public class RecetteRepository : IRecetteRepository
    // Implémentation concrète du repository des recettes.
    {
        const string RECETTE_TABLE = "recette";
        // Nom de la table des recettes en base.

        const string UTILISATEUR_RECETTE_TABLE = "utilisateur_recette";
        // Nom de la table de liaison entre utilisateur et recette.

        readonly IDBSession _dbSession;
        // Session de base de données (connexion + transaction partagée).

        public RecetteRepository(IDBSession dbSession)
        // Constructeur recevant la session via l’injection de dépendances.
        {
            _dbSession = dbSession;
            // Stocke la session pour une utilisation dans toutes les méthodes.
        }

        public async Task<IEnumerable<Recette>> GetAllAsync()
        // Récupère toutes les recettes.
        {
            string query = $"SELECT * FROM {RECETTE_TABLE}";
            // Requête SQL sélectionnant toutes les lignes de la table recette.

            return await _dbSession.Connection.QueryAsync<Recette>(query, transaction: _dbSession.Transaction);
            // Exécution de la requête avec Dapper dans la transaction en cours.
        }

        public async Task<Recette> GetAsync(int id)
        // Récupère une recette par son identifiant.
        {
            string query = $"SELECT * FROM {RECETTE_TABLE} WHERE id = @id";
            // Requête SQL paramétrée filtrant sur la colonne id.

            return await _dbSession.Connection.QuerySingleOrDefaultAsync<Recette>(query, new { id }, transaction: _dbSession.Transaction);
            // Retourne une recette ou null (si aucune trouvée) dans le contexte de la transaction.
        }

        public async Task<Recette> CreateAsync(Recette recette)
        // Crée une nouvelle recette et retourne l’objet avec son Id renseigné.
        {
            string query = string.Empty;
            // Chaîne de requête initialisée vide.

            if (_dbSession.DatabaseProviderName == DatabaseProviderName.MariaDB || _dbSession.DatabaseProviderName == DatabaseProviderName.MySQL)
            {
                // Cas MariaDB / MySQL : récupération de l’ID via LAST_INSERT_ID().
                query = $@"
                    INSERT INTO {RECETTE_TABLE}(nom, temps_preparation, temps_cuisson, difficulte, photo, createur) 
                    VALUES(@Nom, @TempsPreparation, @TempsCuisson, @Difficulte, @Photo, @Createur); 
                    SELECT LAST_INSERT_ID();";
            }
            else if (_dbSession.DatabaseProviderName == DatabaseProviderName.PostgreSQL)
            {
                // Cas PostgreSQL : on utilise RETURNING id.
                query = $@"
                    INSERT INTO {RECETTE_TABLE}(nom, temps_preparation, temps_cuisson, difficulte, photo, createur) 
                    VALUES(@Nom, @TempsPreparation, @TempsCuisson, @Difficulte, @Photo, @Createur) 
                    RETURNING id;";
            }

            int lastId = _dbSession.Connection.ExecuteScalar<int>(query, recette, transaction: _dbSession.Transaction);
            // Exécute la requête d’insertion et récupère la clé primaire générée.

            recette.Id = lastId;
            // Affecte l’Id généré à l’objet recette en mémoire.

            return recette;
            // Retourne la recette nouvellement insérée.
        }

        public async Task<Recette> ModifyAsync(Recette recette)
        // Met à jour une recette existante.
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
            // Requête d’UPDATE paramétrée sur l’Id de la recette.

            int numLine = await _dbSession.Connection.ExecuteAsync(query, recette, transaction: _dbSession.Transaction);
            // Exécute la requête et retourne le nombre de lignes affectées.

            return numLine == 0 ? null : recette;
            // Si aucune ligne modifiée, renvoie null, sinon renvoie la recette mise à jour.
        }

        public async Task<bool> DeleteAsync(int id)
        // Supprime une recette par son Id.
        {
            string query = $"DELETE FROM {RECETTE_TABLE} WHERE id = @id";
            // Requête de suppression filtrée sur la clé primaire.

            int numLine = await _dbSession.Connection.ExecuteAsync(query, new { id }, transaction: _dbSession.Transaction);
            // Exécute la suppression et récupère le nombre de lignes affectées.

            return numLine != 0;
            // Retourne true si au moins une ligne a été supprimée.
        }

        #region Methods specific to RecetteRepository
        // Méthodes spécifiques à la gestion des relations et requêtes avancées sur les recettes.

        public async Task<bool> AddRecetteUtilisateurRelationshipAsync(int idUtilisateur, int idRecette)
        // Ajoute une relation entre un utilisateur et une recette (table de liaison).
        {
            string query = $"INSERT INTO {UTILISATEUR_RECETTE_TABLE}(idutilisateur, idrecette) VALUES(@idUtilisateur, @idRecette)";
            // Requête d’insertion dans la table de liaison.

            int numLine = await _dbSession.Connection.ExecuteAsync(query, new { idUtilisateur, idRecette }, transaction: _dbSession.Transaction);
            // Exécute l’insertion dans le contexte de la transaction.

            return numLine != 0;
            // Retourne true si la relation a bien été créée.
        }

        public async Task<bool> RemoveRecetteUtilisateurRelationshipAsync(int idUtilisateur, int idRecette)
        // Supprime une relation entre un utilisateur et une recette.
        {
            string query = $"DELETE FROM {UTILISATEUR_RECETTE_TABLE} WHERE idutilisateur = @idUtilisateur AND idrecette = @idRecette";
            // Requête de suppression filtrée par utilisateur et recette.

            int numLine = await _dbSession.Connection.ExecuteAsync(query, new { idUtilisateur, idRecette }, transaction: _dbSession.Transaction);
            // Exécute la suppression dans la transaction en cours.

            return numLine != 0;
            // Retourne true si au moins une ligne de la table de liaison a été supprimée.
        }

        public async Task<IEnumerable<Recette>> GetRecettesByIdUtilisateurAsync(int idUtilisateur)
        // Récupère toutes les recettes associées à un utilisateur donné.
        {
            string query = $"SELECT b.* FROM {RECETTE_TABLE} b JOIN {UTILISATEUR_RECETTE_TABLE} ab ON b.id = ab.idRecette WHERE ab.idutilisateur = @idUtilisateur";
            // Jointure entre la table recette et la table de liaison, filtrée sur l’utilisateur.

            return await _dbSession.Connection.QueryAsync<Recette>(query, new { idUtilisateur }, transaction: _dbSession.Transaction);
            // Exécute la requête et mappe les résultats vers des objets Recette.
        }

        public async Task<bool> DeleteRecetteRelationsAsync(int idRecette)
        // Supprime toutes les relations entre une recette et les utilisateurs dans la table de liaison.
        {
            string query = $"DELETE FROM {UTILISATEUR_RECETTE_TABLE} WHERE idrecette = @idRecette";
            // Requête de suppression sur la table de liaison pour un idRecette donné.

            int numLine = await _dbSession.Connection.ExecuteAsync(query, new { idRecette }, transaction: _dbSession.Transaction);
            // Exécute la suppression.

            return numLine != 0;
            // Retourne true si au moins une relation a été supprimée.
        }

        public Task<bool> AddRecetteIngredientRelationshipAsync(int idIngredient, int idRecette)
        // Prévu pour ajouter une relation recette–ingrédient (non implémenté).
        {
            throw new NotImplementedException();
        }

        public Task<bool> RemoveRecetteIngredientRelationshipAsync(int idIngredient, int idRecette)
        // Prévu pour supprimer une relation recette–ingrédient (non implémenté).
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Recette>> GetRecettesByIdIngredientAsync(int idIngredient)
        // Prévu pour récupérer les recettes à partir d’un ingrédient (non implémenté).
        {
            throw new NotImplementedException();
        }

        #endregion Methods specific to RecetteRepository
    }
}
