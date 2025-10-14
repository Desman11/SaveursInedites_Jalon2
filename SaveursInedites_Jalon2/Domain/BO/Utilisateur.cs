namespace SaveursInedites_Jalon2.Domain.BO
{
    public class Utilisateur
    {
        // Identifiant unique (SERIAL en base)
        public int Id { get; set; }

        // Nom de l'utilisateur (VARCHAR(100) NOT NULL)
        public string Identifiant { get; set; } = string.Empty;

        // Email de l'utilisateur (VARCHAR(100) NOT NULL)
        public string Email { get; set; } = string.Empty;

        // Mot de passe de l'utilisateur (VARCHAR(100) NOT NULL)
        public string Password { get; set; } = string.Empty;

        public string Role { get; set; } = "User";


    }
}