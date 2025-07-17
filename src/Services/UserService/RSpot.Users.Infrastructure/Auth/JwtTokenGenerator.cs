using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using RSpot.Users.Domain.Interfaces;
using RSpot.Users.Domain.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using RSpot.Users.Application.Configuration;

namespace RSpot.Users.Infrastructure.Auth
{
    public class JwtTokenGenerator : IJwtTokenGenerator
    {
        private readonly JwtSettings _options;

        public JwtTokenGenerator(IOptions<JwtSettings> opt)
        {
            _options = opt.Value;
            Console.WriteLine($"[JwtTokenGenerator] Secret length: {_options.Secret?.Length ?? 0}");
            Console.WriteLine($"[JwtTokenGenerator] Secret starts with: {_options.Secret?.Substring(0, Math.Min(10, _options.Secret.Length)) ?? "<null>"}");
        }


        public string GenerateToken(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.Secret));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.UtcNow.AddMinutes(_options.ExpiryMinutes);

            var claims = new[]
            {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim(ClaimTypes.Name, user.Name),
            new Claim(ClaimTypes.Role, user.Role)
        };

            var token = new JwtSecurityToken(
                issuer: _options.Issuer,
                audience: _options.Audience,
                claims: claims,
                expires: expires,
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public DateTime GetExpiration(string jwt)
        {
            var handler = new JwtSecurityTokenHandler();
            var token = handler.ReadJwtToken(jwt);
            var expUnix = long.Parse(token.Claims.First(c => c.Type == JwtRegisteredClaimNames.Exp).Value);
            return DateTimeOffset.FromUnixTimeSeconds(expUnix).UtcDateTime;
        }
    }
}
