using KutyApp.Services.Environment.Bll.Dtos;
using KutyApp.Services.Environment.Bll.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace KutyApp.Services.Environment.Api.Controllers
{
    public class AccountController : BaseController
    {
        private IAuthManager AuthManager { get; }

        public AccountController(IAuthManager authManager)
        {
            this.AuthManager = authManager;
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<ActionResult<string>> Login(LoginDto dto) =>
           Result(await AuthManager.GetTokenAsync(dto));

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<ActionResult<string>> Register(RegisterDto dto) =>
           Result(await AuthManager.RegisterAsync(dto));

        [HttpGet("ping")]
        [AllowAnonymous]
        public IActionResult Ping() =>
            Ok();

        [HttpGet("authping")]
        public IActionResult AuthPing() =>
            Ok();

        

        [HttpGet("error")]
        [AllowAnonymous]
        public IActionResult Error(string message) =>
            throw new System.Exception(message);

        [HttpGet("getUser/{name}")]
        public async Task<ActionResult<UserDto>> GetUser(string name) =>
            Result(await AuthManager.GetUserAsync(name));

    }
}