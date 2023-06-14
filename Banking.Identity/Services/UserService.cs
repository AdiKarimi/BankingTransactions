using Banking.Identity.Helpers;
using Banking.Identity.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace Banking.Identity.Services
{
    public interface IUserService
    {
        Models.SecurityToken Authenticate(string username, string password);
    }

    public class UserService : IUserService
    {
        private readonly List<User> _users = new List<User>
        {
            new User { AccountNumber = 1234567, Currency = "USD", FullName = "John Smith", Username = "johnsmith", Password = "john1234" },
            new User { AccountNumber = 7654321, Currency = "USD", FullName = "Anna Brooke", Username = "anna", Password = "anna7654" },
            new User { AccountNumber = 1859624, Currency = "USD", FullName = "Jack White", Username = "jackwhite", Password = "jack1859" }
        };

        private readonly AppSettings _appSettings;

        public UserService(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
        }

        public Models.SecurityToken Authenticate(string username, string password)
        {
            var user = _users.SingleOrDefault(x => x.Username == username && x.Password == password);

            // return null if user not found
            if (user == null)
                return null;

            // authentication successful so generate jwt token
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim("accountnumber", user.AccountNumber.ToString()),
                    new Claim("currency", user.Currency),
                    new Claim("name", user.FullName)
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var jwtSecurityToken = tokenHandler.WriteToken(token);

            return new Models.SecurityToken() { auth_token = jwtSecurityToken };
        }
    }
}
