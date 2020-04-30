using Microsoft.IdentityModel.Tokens;

namespace Domain.TokenGenerator
{
    public class SigningConfigurations
    {
        public SigningCredentials SigningCredentials { get; internal set; }
    }
}