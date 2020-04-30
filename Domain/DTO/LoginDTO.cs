namespace Domain.DTO
{
    public class LoginDTO
    {
        public string Email { get; set; }
        public string Senha { get; set; }
        public string UserID { get; set; }
        public string AccessKey { get; set; }
        public string RefreshToken { get; set; }
        public string GrantType { get; set; }
    }
}
