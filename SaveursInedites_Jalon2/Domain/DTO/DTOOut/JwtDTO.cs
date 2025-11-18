namespace SaveursInedites_Jalon2.Domain.DTO.DTOOut
// Namespace contenant les objets envoyés en sortie par l’API.
{
    public class JwtDTO
    // DTO utilisé pour renvoyer un token JWT au client après authentification.
    {
        public string Token { get; set; }
        // Le token JWT généré par le système d’authentification.
    }
}
