using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using E_Commerce.Configurations;
using E_Commerce.Data;
using E_Commerce.Data.Entities;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.IdentityModel.JsonWebTokens;
using JwtRegisteredClaimNames = System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames;

namespace E_Commerce.Helpers
{
    public class TokenManger
    {
        private readonly IOptions<JWTSettings> _jwtOptions;
        private readonly IRepository<RevokedToken> _repository;

        public TokenManger(IOptions<JWTSettings> jwtOptions , IRepository<RevokedToken> repository)
        {
            _jwtOptions = jwtOptions;
            _repository = repository;
        }

        public string Generate(User user)
        {
            var jwtToken = new JwtSecurityToken(
                signingCredentials: new SigningCredentials( new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_jwtOptions.Value.Secret)) , SecurityAlgorithms.HmacSha512),
                issuer: _jwtOptions.Value.CurrentIssuer,
                audience: _jwtOptions.Value.CurrentAudience,
                expires: DateTime.Now.AddDays(7),
                claims: new []
                {
                    new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.GivenName , user.Name),
                    new Claim(JwtRegisteredClaimNames.AuthTime,DateTime.Now.ToString()),
                    new Claim(JwtRegisteredClaimNames.Email,user.Email),
                    new Claim(JwtRegisteredClaimNames.Sub,user.Id.ToString()),
                    new Claim("UserId",user.Id.ToString()), 
                    new Claim(ClaimTypes.Role,user.Role.ToString()), 
                }
            );

            return new JwtSecurityTokenHandler().WriteToken(jwtToken);
        }

        public async Task Revoke(Guid jti, Guid userId)
        {
            await _repository.Create(new RevokedToken
            {
                Id = jti,
                UserId = userId,
                RevokedAt = DateTime.Now
            });
            await _repository.Save();
        }
        public bool IsTokenRevoked(Guid userId , Guid jti)
        {
            return _repository.Where(t => t.UserId == userId && t.Id == jti).Any();
        }
    }
}