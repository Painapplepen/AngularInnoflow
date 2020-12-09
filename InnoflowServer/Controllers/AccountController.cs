using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using InnoflowServer.Domain.Core.DTO;
using InnoflowServer.Domain.Core.Models;
using InnoflowServer.Services.Interfaces.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace InnoflowServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IUserService _service;
        private readonly IMapper _mapper;

        public AccountController(IUserService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        [Route("/register")]
        [HttpPost]
        public async Task<IActionResult> Register([FromBody] RegisterUserModel model)
        {
            var code = await _service.Register(_mapper.Map<UserDTO>(model));
            if (code == null)
            {
                return BadRequest();
            }
            string callbackUrl = Url.Action(
                "ConfirmEmail",
                "Account",
                new { userEmail = model.Email, code = code },
                protocol: HttpContext.Request.Scheme);
            string message = $"Подтвердите регистрацию, перейдя по ссылке: <a href = '{callbackUrl}'>Link</a>";
            if (!await _service.SendEmail(_mapper.Map<UserDTO>(model), message))
            {
                return BadRequest();
            }
            return Ok();
        }

        [Route("/ConfirmEmail")]
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(string userEmail, string code)
        {

            if (!await _service.ConfirmEmail(userEmail, code))
            {
                return BadRequest();
            }

            return Ok();
        }
        [Route("/login")]
        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginUserModel model)
        {
            var tokenStr = await _service.Login(_mapper.Map<UserDTO>(model));
            if (tokenStr.Length > 0)
            {
                return Ok(new { token = tokenStr });
            }
            return BadRequest();
        }

        [Route("/ChangeProfile")]
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> ChangeProfile([FromBody] ChangeProfileUserModel model)
        {
            return Ok();
        }

        [Route("/GetProfileData")]
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetProfileData(string email)
        {
            var result = await _service.GetProfile(email);
            return Ok(new { FirstName = result.FirstName, LastName = result.LastName, PhoneNumber = result.PhoneNumber, Email = result.Email, Role = result.Role });
        }

        [Route("/logout")]
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _service.Logout();
            return Ok();
        }

        [Route("/GetAssociatedCases")]
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAssociatedCases(string email)
        {
            return Ok(new { message = "not implemented" });
        }
    }
}