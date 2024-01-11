using Coursewise.Security.Compact.Interfaces;
using Coursewise.Security.Compact.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Coursewise.Security.Compact.Services
{
    public class TokenGeneratorService : ITokenGenerator
    {
        private readonly TokenProviderOptions _tokenProviderOptions;
        public TokenGeneratorService(IOptions<TokenProviderOptions> tokenProviderOptions)
        {
            _tokenProviderOptions = tokenProviderOptions.Value;
        }
        public Task<AccessToken> GenerateToken(string userId, string email, IEnumerable<Claim> claims)
        {
            var now = DateTime.UtcNow;
            var fixedClaims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub,userId),
                new Claim(JwtRegisteredClaimNames.Email,email),
                new Claim(JwtRegisteredClaimNames.Iat,new DateTimeOffset(now)
                                    .ToUniversalTime()
                                    .ToUnixTimeSeconds()
                                    .ToString(),
                                     ClaimValueTypes.Integer64),
            };

            if (claims.Any())
            {
                foreach (var claim in claims)
                {
                    if (claim.Type == ClaimTypes.Role)
                    {
                        fixedClaims.Add(new Claim("role", claim.Value));
                    }
                    else
                    {
                        fixedClaims.Add(claim);
                    }
                }
            }

            var sharedKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_tokenProviderOptions.Secret));
            var signingCredentials = new SigningCredentials(sharedKey, SecurityAlgorithms.HmacSha256);

            var expiry = now.AddMinutes(_tokenProviderOptions.Expiration);



            var jwt = new JwtSecurityToken(
               issuer: _tokenProviderOptions.Issuer,
               audience: _tokenProviderOptions.Audience,
               claims: fixedClaims,
               notBefore: now,
               expires: expiry,
                signingCredentials: signingCredentials
               );

            var handler = new JwtSecurityTokenHandler();
            handler.InboundClaimTypeMap.Clear();

            var encodedJwt = handler.WriteToken(jwt);

            var response = new AccessToken
            {
                Token = encodedJwt,
                Expiry = jwt.Claims.FirstOrDefault(x => x.Type == "exp")?.Value!
            };

            return Task.FromResult(response);
        }
    }
}