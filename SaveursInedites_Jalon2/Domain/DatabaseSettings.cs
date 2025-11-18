using FluentValidation;
// Importation de FluentValidation pour définir les règles de validation.

using SaveursInedites_Jalon2.DataAccessLayer.Session;
// Import potentiellement utilisé par d'autres composants mais non directement ici.

namespace SaveursInedites_Jalon2.Domain
// Namespace contenant les objets liés à la configuration et aux règles du domaine.
{
    public enum DatabaseProviderName
    // Énumération listant les fournisseurs de base de données supportés.
    {
        MariaDB,
        MySQL,
        SQLServer,
        PostgreSQL,
        Oracle
    }

    public class DatabaseSettings : IDatabaseSettings
    // Classe contenant la configuration de la base de données.
    // Implémente l’interface IDatabaseSettings.
    {
        public string ConnectionString { get; set; }
        // Chaîne de connexion utilisée pour accéder à la base de données.

        public DatabaseProviderName? DatabaseProviderName { get; set; }
        // Type de base de données utilisée (PostgreSQL, MySQL, etc.). Nullable pour permettre
        // un état “non défini” avant validation.
    }

    public class DatabaseSettingsValidator : AbstractValidator<DatabaseSettings>
    // Validateur FluentValidation pour s’assurer que DatabaseSettings est correct.
    {
        public DatabaseSettingsValidator()
        // Constructeur dans lequel on définit les règles de validation.
        {
            var connectionMessage = "La chaîne de connexion est invalide.";
            // Message commun utilisé en cas d’erreur sur la chaîne de connexion.

            var providerMessage =
                $"Le type de base de données est invalide. Valeurs possibles : {string.Join(", ", Enum.GetNames(typeof(DatabaseProviderName)))}";
            // Message indiquant que le provider n'est pas valide, avec liste des valeurs acceptées.

            RuleFor(x => x.ConnectionString)
                .Cascade(CascadeMode.Stop)
                // Stoppe la validation sur cette règle dès qu’une condition échoue.

                .NotNull().WithMessage(connectionMessage)
                // La chaîne de connexion doit être renseignée.

                .NotEmpty().WithMessage(connectionMessage);
            // La chaîne ne doit pas être une chaîne vide.

            RuleFor(x => x.DatabaseProviderName)
                .Cascade(CascadeMode.Stop)
                // Stoppe la règle dès la première erreur.

                .NotNull().WithMessage(providerMessage)
                // Le type de base de données doit être sélectionné.

                .IsInEnum().WithMessage(providerMessage);
            // Vérifie que la valeur correspond bien à une valeur de l’énumération.
        }
    }
}
