using DAL.Entity;
using DAL.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace API.Services
{
    public interface IAuthService
    {
        Task<(int, string)> Registeration(RegistrationModel model);
        Task<LoginResult> Login(LoginModel model);
        Task<(int, string)> ChangePassword(ChangePasswordModel model);
        Task<(int, string)> UpdateProfile(UpdateProfileModel model, string userId);


    }

    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IConfiguration _configuration;
        public AuthService(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            _configuration = configuration;

        }
        public async Task<(int, string)> Registeration(RegistrationModel model)
        {
            var userExists = await userManager.FindByNameAsync(model.Username);
            if (userExists != null)
                return (0, "User already exists");

            ApplicationUser user = new ApplicationUser()
            {
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.Username,
                FirstName = model.FirstName,
                LastName=model.LastName,
                Mobile=model.Mobile
            };
            var createUserResult = await userManager.CreateAsync(user, model.Password);
            if (!createUserResult.Succeeded)
                return (0, createUserResult.Errors.FirstOrDefault().Description);            

            return (1, "User created successfully!");
        }

        public async Task<LoginResult> Login(LoginModel model)
        {
            var user = await userManager.FindByNameAsync(model.Username);
            if (user == null)
                throw new Exception("Invalid username");

            if (!await userManager.CheckPasswordAsync(user, model.Password))
                    throw new Exception("Invalid password");
                        
            var authClaims = new List<Claim>
            {
               new Claim(ClaimTypes.NameIdentifier, user.Id),
               new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };
            string token = GenerateToken(authClaims);


            var res = new LoginResult();
            res.token = token;
            res.userId = user.Id;
            res.username = user.UserName;
            res.firstname = user.FirstName;
            res.lastname = user.LastName;
            res.mobile = user.Mobile;

            return (res);
        }       

        public async Task<(int, string)> ChangePassword(ChangePasswordModel model)
        {
            var user = await userManager.FindByNameAsync(model.Username);

            if (user == null)
                return (0, "Invalid username");

            var token = await userManager.GeneratePasswordResetTokenAsync(user);

            var result = await userManager.ResetPasswordAsync(user, token, model.NewPassword);

            if (result.Succeeded)
            {
                return (1, "User password changed successfully!");
            }
            else
            {
                return (0, "Change password failed!");
            }
        }

        public async Task<(int, string)> UpdateProfile(UpdateProfileModel model,string userId)
        {
            var user = await userManager.FindByIdAsync(userId);

            if (user == null)
                return (0, "Invalid username");

            user.FirstName = model.FirstName?? user.FirstName;
            user.LastName = model.LastName?? user.LastName;
            user.Mobile = model.Mobile?? user.Mobile;
            var result=await userManager.UpdateAsync(user);

            if (result.Succeeded)
            {
                return (1, "User updated successfully!");
            }
            else
            {
                return (0, "update user failed!");
            }
        }


        private string GenerateToken(IEnumerable<Claim> claims)
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = _configuration["JWT:ValidIssuer"],
                Audience = _configuration["JWT:ValidAudience"],
                Expires = DateTime.UtcNow.AddHours(3),
                SigningCredentials = new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256),
                Subject = new ClaimsIdentity(claims)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
