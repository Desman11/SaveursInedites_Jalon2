// Interface définissant le contrat d’un service responsable de la génération de tokens JWT.
// Elle déclare une méthode unique : GenerateToken.
// - username : identifiant de l’utilisateur pour lequel le token doit être généré.
// - roles : liste de rôles associés à l’utilisateur, utilisée pour inclure les claims d’autorisation dans le JWT.
// Le service qui implémentera cette interface devra produire une chaîne représentant un token signé.
namespace SaveursInedites_Jalon2.Services
{
    public interface IJwtTokenService
    {
        string GenerateToken(string username, params string[] roles);
    }
}
