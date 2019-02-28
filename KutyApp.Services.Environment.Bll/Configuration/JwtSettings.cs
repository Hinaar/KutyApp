namespace KutyApp.Services.Environment.Bll.Configuration
{
    public class JwtSettings
    {
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public int ExpiryDays { get; set; }
        public int ExpiricyInMinutes { get; set; }
        public string PrivateKey { get; set; }
        public string PublicKey { get; set; }
    }
}
