using DataAccess.Enum;
using DataAccess.Interface;
using DataAccess.Models;
using DataAccess.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Practical_19.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace Practical_19.Repository
{
    public class UserRepository : IUserRepo
    {
        private readonly UserManager<AppAuthentiction> _userManager;
        private readonly IConfiguration _configuration;


        public UserRepository(UserManager<AppAuthentiction> userManager,IConfiguration configuration)
        {
            _userManager = userManager;
            _configuration = configuration;
         
        }


        public async Task<UserManagerResponse> LoginUserAsync(LoginViewModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                return new UserManagerResponse
                {
                    Message = "There is no user with this email try again!",
                    IsSuccess = false,
                };
            }
            var result = await _userManager.CheckPasswordAsync(user, model.Password);
            if (!result)
            {
                return new UserManagerResponse
                {
                    Message = "Invalid Password try again!",
                    IsSuccess = false,
                };
            }
            var roles = await _userManager.GetRolesAsync(user);

            var claims = new List<Claim>()
            {
                new Claim("Email", model.Email),
                new Claim(ClaimTypes.NameIdentifier, user.Id),

            };
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role.ToString()));
            }

            var keys = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Secret"]));

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(2),
                signingCredentials: new SigningCredentials(keys, SecurityAlgorithms.HmacSha256)
                );
            string tokenAsString = new JwtSecurityTokenHandler().WriteToken(token);
            return new UserManagerResponse
            {
                Message = tokenAsString,
                IsSuccess = true,
                ExpireDate = token.ValidTo,
                Email = user.Email,
            };
        }
        public async Task<UserManagerResponse> RegisterUserAsync(Users model)
        {
            var email = await _userManager.FindByEmailAsync(model.Email);
            if (email != null)
            {
                return new UserManagerResponse
                {
                    Message = $"{email} already exist!"
                };
            }
            var IdetityUser = new IdentityUser()
            {
                Email = model.Email,
                UserName = model.Email
            };
            var result = await _userManager.CreateAsync((AppAuthentiction)IdetityUser, model.Password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync((AppAuthentiction)IdetityUser, "User");

                return new UserManagerResponse
                {
                    Message = "User Created Successfully!",
                    IsSuccess = true,
                };
            }

            return new UserManagerResponse
            {
                Message = "User not created try again!",
                IsSuccess = false,
                Errors = result.Errors.Select(e => e.Description)
            };
        }
        public async Task<UserManagerResponse> LogoutUserAsync(LogoutViewModel model)
        {
            return new UserManagerResponse
            {
                Message = "User Signout",
                IsSuccess = true,
            };
        }
    }
}
