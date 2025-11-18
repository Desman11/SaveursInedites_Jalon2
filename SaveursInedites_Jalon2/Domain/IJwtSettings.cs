namespace SaveursInedites_Jalon2.Domain
// Espace de noms regroupant les objets de configuration et les éléments du domaine.
{
    public interface IJwtSettings
    // Interface qui définit le contrat des paramètres nécessaires pour configurer le système JWT.
    {
        public string Secret { get; }
        // Clé secrète utilisée pour signer les tokens JWT.
        // Doit être suffisamment longue et sécurisée.

        public string Issuer { get; }
        // Identifiant de l’émetteur du token, généralement le nom de l’API.

        public string Audience { get; }
        // Identifiant du client prévu pour consommer les tokens (WinForms, API, web, etc.).

        public int ExpirationMinutes { get; }
        // Durée de validité du token en minutes.
    }
}
