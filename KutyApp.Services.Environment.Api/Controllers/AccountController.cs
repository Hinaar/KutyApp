using KutyApp.Services.Environment.Bll.Dtos;
using KutyApp.Services.Environment.Bll.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace KutyApp.Services.Environment.Api.Controllers
{
    public class AccountController : BaseController
    {
        public IAuthManager AuthManager { get; }

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
        public IActionResult Ping() =>
            Ok();
    }
}