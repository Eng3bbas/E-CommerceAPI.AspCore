namespace E_Commerce.Configurations
{
    public class JWTSettings
    {
        public string Secret { get; set; }
        public string CurrentIssuer { get; set; }
        public string CurrentAudience { get; set; }
    }
}