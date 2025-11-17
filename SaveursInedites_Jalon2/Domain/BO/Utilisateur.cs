namespace SaveursInedites_Jalon2.Domain.BO
{
    public class Utilisateur
    {
        public int Id { get; set; }


        public string Identifiant { get; set; }


        public string Email { get; set; }


        public string Password { get; set; }

        public int Role_id { get; set; }

    }
}