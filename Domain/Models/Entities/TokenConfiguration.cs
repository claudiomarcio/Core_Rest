namespace Domain.Models.Entities
{
    public class TokenConfiguration
    {
        public string Audience { get; set; }
        public string Issuer { get; set; }
        public int Seconds { get; set; }
        public int FinalExpiration { get; set; }
    }

    public class AccessCredentials
    {
        public string UserID { get; set; }
        public string AccessKey { get; set; }
        public string RefreshToken { get; set; }
        public string GrantType { get; set; }
    }

    public class User
    {
        public string UserID { get; set; }
        public string AccessKey { get; set; }
    }

    public class RefreshTokenData
    {
        public string RefreshToken { get; set; }
        public string UserID { get; set; }
    }
}