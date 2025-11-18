namespace SaveursInedites_Jalon2.Domain.DTO.DTOOut
// Namespace dédié aux DTO envoyés depuis l’API vers le client (DTO de sortie).
{
    public class UtilisateurDTO
    // Représentation simplifiée d’un utilisateur destinée à être retournée au client.
    {
        public int Id { get; set; }
        // Identifiant unique de l’utilisateur.

        public string Identifiant { get; set; }
        // Nom d’utilisateur / login visible côté client.

        public string Email { get; set; }
        // Adresse e-mail de l’utilisateur.

        public string Password { get; set; }
        // Mot de passe reçu ou renvoyé.
        // À noter : en pratique, un DTO de sortie ne devrait jamais exposer un mot de passe.

        public int Role_id { get; set; }
        // Identifiant du rôle associé à l’utilisateur (ex. admin, user...).
    }
}
