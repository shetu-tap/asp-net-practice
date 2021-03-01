using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AuthenticationDemo.Dto;
using AuthenticationDemo.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace AuthenticationDemo.Services
{
    public class UserService: IUserService
    {

        private readonly UserManager<IdentityUser> _userManager;
        private readonly IConfiguration _configuration;
        public UserService(UserManager<IdentityUser> userManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _configuration = configuration;
        }

        public async Task<RegistrationResponse> createUserAsync(RegistrationViewModel model)
        {

            if(model == null)
            {
                throw new NullReferenceException("Registration model is null");
            }

            if(model.ConfirmPassword != model.Password)
            {
                return new RegistrationResponse
                {
                    Message = "Password did not match",
                    isSuccess = false
                };
            }

            IdentityUser idenity = new IdentityUser
            {
                Email = model.Email,
                UserName = model.Email
            };

            var result = await _userManager.CreateAsync(idenity, model.Password);
            if(result.Succeeded)
            {
                return new RegistrationResponse
                {
                    Message = "User created successfully",
                    isSuccess = true
                };
            }
            return new RegistrationResponse
            {
                Message = "User is not created successfully",
                isSuccess = false
            };
        }

        public async Task<LoginResponse> loginAsync(LoginViewModel model)
        {
            if(model == null)
            {
                throw new NullReferenceException("Login model is not present");
            }

            var identity = await _userManager.FindByEmailAsync(model.Email);
            if(identity == null)
            {
                return new LoginResponse
                {
                    Message = "User not found",
                    isSuccess = false
                };
            }
            var result = await _userManager.CheckPasswordAsync(identity, model.Password);
            if(!result)
            {
                return new LoginResponse
                {
                    Message = "Password did not match",
                    isSuccess = false
                };

            }

            // create token now
            var claims = new[]
            {
                new Claim("Email", identity.Email),
                new Claim(ClaimTypes.NameIdentifier, identity.Id)
            };

            var token = new JwtSecurityToken
            (
                issuer: _configuration["Authorization:Issuer"],
                audience: _configuration["Authorization:Audience"],
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Authorization:Key"])), SecurityAlgorithms.HmacSha256),
                expires: DateTime.Now.AddDays(30),
                claims: claims
            );

            string tokenAsString = new JwtSecurityTokenHandler().WriteToken(token);

            return new LoginResponse
            {
                Token = tokenAsString,
                ExpiresIn = token.ValidTo,
                Message = "Login is successful",
                isSuccess = true
            };
        }
    }
}
