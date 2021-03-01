using System;
using System.ComponentModel.DataAnnotations;

namespace AuthenticationDemo.Dto
{
    public class RegistrationViewModel
    {
        [Required]
        [EmailAddress]
        [StringLength(50)]
        public string Email { get; set; }
        [Required]
        [StringLength(50, MinimumLength =5)]
        public string Password { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 5)]
        public string ConfirmPassword { get; set; }
    }

    public class RegistrationResponse
    {
        public string Message { get; set; }
        public bool isSuccess { get; set; }
    }
}
