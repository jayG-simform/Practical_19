using Azure;
using DataAccess.Interface;
using DataAccess.Models;
using DataAccess.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Practical_19.Data;
using Practical_19.Repository;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Practical_19.Controllers
{
    [Route("api/{controller}")]
    [ApiController]
    public class ActionController : ControllerBase
    {
        private readonly IUserRepo _userRepo;

        public ActionController(IUserRepo userRepo)
        {
            _userRepo = userRepo;
        }
        [HttpPost("Register")]
        public async Task<ActionResult> PostUsers(Users users)
        {
            if (ModelState.IsValid)
            {
                var result = await _userRepo.RegisterUserAsync(users);
                if (result.IsSuccess)
                {
                    return Ok(result);
                }
                else
                {
                    return BadRequest(result.Message);
                }
            }
            return BadRequest("Properties are not valid");
        }
        [HttpPost("Login")]
        public async Task<ActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _userRepo.LoginUserAsync(model);

                if (result.IsSuccess)
                {
                    return Ok(result);
                }
                else
                {
                    return BadRequest(result);
                }
            }
            return BadRequest("Properties are not valid");
        }
        [HttpPost("Logout")]
        public async Task<IActionResult> LogoutAsync(LogoutViewModel model)
        {
            await _userRepo.LogoutUserAsync(model);
            return Ok();

        }

    }
}
