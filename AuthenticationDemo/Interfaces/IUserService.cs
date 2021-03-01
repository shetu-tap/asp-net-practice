using System;
using System.Threading.Tasks;
using AuthenticationDemo.Dto;

namespace AuthenticationDemo.Interfaces
{
    public interface IUserService
    {
        Task<RegistrationResponse> createUserAsync(RegistrationViewModel model);
        Task<LoginResponse> loginAsync(LoginViewModel model);
    }
}
