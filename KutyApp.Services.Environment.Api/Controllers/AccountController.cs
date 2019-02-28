using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KutyApp.Services.Environment.Bll.Entities.Model;
using KutyApp.Services.Environment.Bll.Interfaces;
using KutyApp.Services.Environment.Bll.Interfaces.Managers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
        public async Task<ActionResult<string>> Login(string username, string password) =>
           Result(await AuthManager.GetTokenAsync(username, password));

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<ActionResult<string>> Register(string email, string password, string confirmPassword) =>
           Result(await AuthManager.RegisterAsync(email, password, confirmPassword));

        [HttpGet("ping")]
        [Authorize]
        public IActionResult Ping() =>
            Ok();
    }
}