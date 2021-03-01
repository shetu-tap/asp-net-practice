using System;
using System.Security.Claims;

namespace AuthenticationDemo.Dto
{
    public class LoginViewModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class LoginResponse
    {
        public string Token { get; set; }
        public string Message { get; set; }
        public bool isSuccess { get; set; }
        public DateTime? ExpiresIn { get; set; }
    }

    public class MeResponse
    {
        public string Email { get; set; }
        public string Identifier { get; set; }
    }
}
