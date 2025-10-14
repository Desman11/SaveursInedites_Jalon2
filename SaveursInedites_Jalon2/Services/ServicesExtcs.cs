namespace SaveursInedites_Jalon2.Services
{
    public static class ServicesExt
    {
        public static void AddBll(this IServiceCollection services)
        {
            services.AddTransient<ISaveursService, SaveursService>();
            services.AddTransient<IJwtTokenService, JwtTokenService>();
        }
    }
}