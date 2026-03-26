using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Domain.Interfaces;
using Domain.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Persistence.Repositories
{
    public class JwtTokenGenerator : IJwtTokenGenerator
    {
        private readonly IConfiguration _configuration;

        public JwtTokenGenerator(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public string GenerateToken(AppUser user)
        {
            var claims = new[]
                {
                    new Claim("UserId", user.Id),
                    new Claim(JwtRegisteredClaimNames.Email, user.Email!),
                    new Claim(JwtRegisteredClaimNames.Name, user.FirstName!),
                    new Claim("CardCode", user.CardCode!),
                    new Claim(JwtRegisteredClaimNames.UniqueName, user.Email!)
                };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration["AppSettings:JWTSecret"]!));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["AppSettings:Issuer"],
                audience: _configuration["AppSettings:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(60),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
