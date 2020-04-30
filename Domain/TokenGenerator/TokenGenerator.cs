using Domain.Models.Entities;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Principal;

namespace Domain.TokenGenerator
{
    public static class TokenGenerator
    {
        public static string GetToken(Usuario usuario, TokenConfiguration tokenConfigurations, dynamic signingConfigurations)
        {

            ClaimsIdentity identity = new ClaimsIdentity(
                new GenericIdentity(usuario.Email, "Login"),
                new[] {
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString("N")),
                        new Claim(JwtRegisteredClaimNames.UniqueName, usuario.Email)
                }
            );

            DateTime dataCriacao = DateTime.Now;
            DateTime dataExpiracao = dataCriacao +
                TimeSpan.FromSeconds(tokenConfigurations.Seconds);

            var handler = new JwtSecurityTokenHandler();
            var securityToken = handler.CreateToken(new SecurityTokenDescriptor
            {
                Issuer = tokenConfigurations.Issuer,
                Audience = tokenConfigurations.Audience,
                SigningCredentials = signingConfigurations.SigningCredentials,
                Subject = identity,
                NotBefore = dataCriacao,
                Expires = dataExpiracao
            });
            var token = handler.WriteToken(securityToken);

            return token;

        }

        public static object GenerateToken(string userID,
           SigningCredentials credentials,
           TokenConfiguration tokenConfigurations,
           IDistributedCache cache)
        {
            ClaimsIdentity identity = new ClaimsIdentity(
                new GenericIdentity(userID, "Login"),
                new[] {
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString("N")),
                        new Claim(JwtRegisteredClaimNames.UniqueName, userID)
                }
            );

            DateTime dataCriacao = DateTime.Now;
            DateTime dataExpiracao = dataCriacao +
                TimeSpan.FromSeconds(tokenConfigurations.Seconds);

            // Calcula o tempo máximo de validade do refresh token
            // (o mesmo será invalidado automaticamente pelo Redis)
            tokenConfigurations.FinalExpiration = 3600;

            TimeSpan finalExpiration =
                TimeSpan.FromSeconds(tokenConfigurations.FinalExpiration);

            var handler = new JwtSecurityTokenHandler();
            var securityToken = handler.CreateToken(new SecurityTokenDescriptor
            {
                Issuer = tokenConfigurations.Issuer,
                Audience = tokenConfigurations.Audience,
                SigningCredentials = credentials,
                Subject = identity,
                NotBefore = dataCriacao,
                Expires = dataExpiracao
            });
            var token = handler.WriteToken(securityToken);

            var resultado = new
            {
                authenticated = true,
                created = dataCriacao.ToString("yyyy-MM-dd HH:mm:ss"),
                expiration = dataExpiracao.ToString("yyyy-MM-dd HH:mm:ss"),
                accessToken = token,
                refreshToken = Guid.NewGuid().ToString().Replace("-", String.Empty),
                message = "OK"
            };

            // Armazena o refresh token em cache através do Redis 
            var refreshTokenData = new RefreshTokenData();
            refreshTokenData.RefreshToken = resultado.refreshToken;
            refreshTokenData.UserID = userID;

            DistributedCacheEntryOptions opcoesCache =
                new DistributedCacheEntryOptions();
            opcoesCache.SetAbsoluteExpiration(finalExpiration);         
            cache.SetString(resultado.refreshToken,
                JsonConvert.SerializeObject(refreshTokenData),
                opcoesCache);

            return resultado;
        }

    }
}
