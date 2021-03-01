using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AuthenticationDemo.Dto;
using AuthenticationDemo.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AuthenticationDemo.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;
        public AuthController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("register")]
        public async Task<ActionResult<RegistrationResponse>> CreateUserAsync([FromBody] RegistrationViewModel model)
        {
            if(ModelState.IsValid)
            {
                RegistrationResponse r = await _userService.createUserAsync(model);
                if(r.isSuccess)
                {
                    return Ok(r);
                }
                return BadRequest(r);
            }
            return BadRequest();
        }

        [HttpPost("login")]
        public async Task<ActionResult<LoginResponse>> loginUserAsync(LoginViewModel model)
        {
            if(ModelState.IsValid)
            {
                LoginResponse r = await _userService.loginAsync(model);
                if (r.isSuccess)
                {
                    return Ok(r);
                }
                return BadRequest(r);
            }
            return BadRequest();
        }

        [HttpPost("me")]
        public ActionResult meUser()
        {
            var identifier = User.FindFirst(ClaimTypes.NameIdentifier);
            var email = User.FindFirst("Email");
            return Ok(new MeResponse {
                Identifier = identifier.Value,
                Email = email.Value
            });
        }
    }

}
