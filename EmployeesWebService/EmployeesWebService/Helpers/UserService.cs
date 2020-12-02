using System.Linq;
using System.Collections.Generic;
using EmployeesWebService.Models;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace EmployeesWebService.Helpers
{
    public class UserService : IUserService
    {
        private List<User> _users = new List<User>()
        {
            new User ()
            {
                Id = 1,
                FirstName = "Daenarys",
                LastName = "Targaryen",
                UserName = "danytargaryen",
                Password = "fireandblood",
                Token = ""
            }
        };

        private readonly JWTSettings _jwtSettings;

        public UserService (IOptions<JWTSettings> jwtSettings)
        {
            _jwtSettings = jwtSettings.Value;
        }

        public User Authenticate (string username, string password)
        {
            var user = _users.SingleOrDefault (x => x.UserName == username && x.Password == password);
            if (user == null) return null;

            var tokenHandler = new JwtSecurityTokenHandler ();
            var key = System.Text.Encoding.ASCII.GetBytes(_jwtSettings.Secret);

            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new System.Security.Claims.ClaimsIdentity(new Claim[] {
                    new Claim (ClaimTypes.Name, user.Id.ToString ())
                }),
                Expires = DateTime.Now.AddDays (1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            user.Token = tokenHandler.WriteToken(token);

            user.Password = null;
            return user;
        }

        public IEnumerable<User> GetAll ()
        {
            return _users.Select (x => {
                x.Password = null;
                return x;
            });
        }
    }
}
