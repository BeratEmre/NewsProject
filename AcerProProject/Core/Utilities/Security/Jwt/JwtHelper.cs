using Core.Entities.Concrete;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
namespace Core.Utilities.Security.Jwt
{
    public class JwtHelper : ITokenHelper
    {
        public IConfiguration Configuration { get; }
        TokenOptions _tokenOptions=new TokenOptions() ;
        DateTime _accesTokenExpiration;
        public JwtHelper(IConfiguration configuration)
        {
            Configuration = configuration;
            var token =
            _tokenOptions.Audience = Configuration.GetSection("TokenOptions:Audience").Value;
            _tokenOptions.Issuer = Configuration.GetSection("TokenOptions:Issuer").Value;
            _tokenOptions.SecurityKey = Configuration.GetSection("TokenOptions:SecurityKey").Value;
            _tokenOptions.AccessTokenExpiretion = Convert.ToInt32(Configuration.GetSection("TokenOptions:AccessTokenExpiration").Value);
            _accesTokenExpiration = DateTime.Now.AddMinutes(_tokenOptions.AccessTokenExpiretion);

        }
        public AccessToken CreateToken(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_tokenOptions.SecurityKey));
            var signingCredentials=new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);
            var jwt = CreateJwtToken(_tokenOptions, user, signingCredentials);
            var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            var token = jwtSecurityTokenHandler.WriteToken(jwt);
            return new AccessToken
            {
                Token = token,
                Expiration = _accesTokenExpiration,                
            };
        }
        public JwtSecurityToken CreateJwtToken(TokenOptions tokenOptions,User user,SigningCredentials signingCredentials)
        {
            _accesTokenExpiration = DateTime.Now.AddMinutes(_tokenOptions.AccessTokenExpiretion);

            var jwt = new JwtSecurityToken(
                issuer: _tokenOptions.Issuer,
                audience: _tokenOptions.Audience,
                expires: _accesTokenExpiration,
                claims: SetClaims(user),
                notBefore:DateTime.Now,
                signingCredentials: signingCredentials
                );
            return jwt;
        }

        private IEnumerable<Claim> SetClaims(User user)
        {
            var claims = new List<Claim>();

            claims.Add(new Claim("firstName", user.FirstName));
            claims.Add(new Claim("id", user.Id.ToString()));
            claims.Add(new Claim("email", user.Email.ToString()));
            return claims;
        }

        public string GetJWTTokenClaim(string token, string claimName)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var securityToken = (JwtSecurityToken)tokenHandler.ReadToken(token);
                var claimValue = securityToken.Claims.FirstOrDefault(c => c.Type == claimName)?.Value;
                return claimValue;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public bool IsTokenValid(string token)
        {
            string key = Configuration.GetSection("TokenOptions:SecurityKey").Value;
            string issuer = Configuration.GetSection("TokenOptions:Issuer").Value;
            //var expires= DateTime.Now.AddMinutes(_tokenOptions.AccessTokenExpiretion);
            var mySecret = Encoding.UTF8.GetBytes(key);
            var mySecurityKey = new SymmetricSecurityKey(mySecret);
            var tokenHandler = new JwtSecurityTokenHandler();
            try
            {
                var a=tokenHandler.ValidateToken(token,
                new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidIssuer = issuer,
                    ValidAudience = issuer,
                    IssuerSigningKey = mySecurityKey,
                }, out SecurityToken validatedToken);
            }
            catch
            {
                return false;
            }
            return true;
        }
    }
}
