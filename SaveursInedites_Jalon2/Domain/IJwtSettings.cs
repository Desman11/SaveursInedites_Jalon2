namespace SaveursInedites_Jalon2.Domain
{
    public class IJwtSettings
    {
        public string Secret { get; }
        public string Issuer { get; }
        public string Audience { get; }
        public int ExpirationMinutes { get; }
    }
}
