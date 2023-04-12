using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using WebApplication.Contracts.Responses;
using WebApplication.Data;
using WebApplication.Options;

namespace WebApplication.Services
{
    public interface IAuthService
    {
        Task<User> LoginAsync(string username, string password);
        Task<AuthenticationResult> GenerateJwtToken(User user);
    }

    public class AuthService : IAuthService
    {
        private readonly WebapplicationContext _dbContext;
        private readonly JwtSettings _jwtSettings;

        public AuthService(WebapplicationContext dbContext, IOptions<JwtSettings> jwtSettings)
        {
            _dbContext = dbContext;
            _jwtSettings = jwtSettings.Value;

        }
        public async Task<User> LoginAsync(string email, string password)
        {
            var user = await _dbContext.Users.SingleOrDefaultAsync(u => u.Email == email && u.Password == password);

            return user;
        }

        public async Task<AuthenticationResult> GenerateJwtToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Secret));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddMinutes(_jwtSettings.TokenLifeTime);

            var claims = new[]
            {
               new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
               new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var tokenDescriptor = new JwtSecurityToken(
                claims: claims,
                expires: expires,
                signingCredentials: credentials
            );
            return new AuthenticationResult
            {
                Success = true,
                Token = tokenHandler.WriteToken(tokenDescriptor),

                User = new LoginUserResponse
                {
                    //Id = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    Id = user.Id,
                },
            };
        }
    }
}
