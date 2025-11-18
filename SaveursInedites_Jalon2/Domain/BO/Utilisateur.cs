namespace SaveursInedites_Jalon2.Domain.BO
// Namespace contenant les objets métiers (Business Objects) du domaine.
{
    public class Utilisateur
    // Classe représentant un utilisateur dans le modèle métier.
    {
        public int Id { get; set; }
        // Identifiant unique de l’utilisateur en base de données.

        public string Identifiant { get; set; }
        // Nom d’utilisateur (login) utilisé pour l’authentification.

        public string Email { get; set; }
        // Adresse email de l’utilisateur.

        public string Password { get; set; }
        // Mot de passe enregistré (idéalement hashé).
        // À ce niveau, le BO stocke souvent la version chiffrée/hashée.

        public int Role_id { get; set; }
        // Identifiant du rôle attribué à l’utilisateur (ex. 1 = user, 2 = admin).
    }
}
