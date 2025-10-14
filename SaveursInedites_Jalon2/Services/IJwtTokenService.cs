namespace SaveursInedites_Jalon2.Services
{
    public interface IJwtTokenService
    {
        string GenerateToken(string username, params string[] roles);
    }
}

