using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;
using KutyApp.Services.Environment.Api.Extensions;
using KutyApp.Services.Environment.Bll.Configuration;
using KutyApp.Services.Environment.Bll.Entities.Model;
using KutyApp.Services.Environment.Bll.Interfaces.Managers;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace KutyApp.Services.Environment.Api.Managers
{
    public class AuthManager : IAuthManager
    {
        private SignInManager<User> SignInManager { get; }
        private UserManager<User> UserManager { get; }
        private JwtSettings JwtSettings { get; }

        public AuthManager(SignInManager<User> signInManager, UserManager<User> userManager, IOptions<JwtSettings> jwtSettings)
        {
            SignInManager = signInManager;
            UserManager = userManager;
            JwtSettings = jwtSettings.Value;
        }
        public async Task<string> GetTokenAsync(string userName, string password)
        {
            var result = await SignInManager.PasswordSignInAsync(userName, password, false, false);

            if (result.Succeeded)
            {
                //var user = await UserManager.FindByNameAsync(userName);
                var user = await UserManager.FindByEmailAsync(userName);

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.Sid, user.Id.ToString())
                };

                SigningCredentials signingCredentials;
                using (RSA privateRsa = RSA.Create())
                {
                    privateRsa.LoadFromXmlString(JwtSettings.PrivateKey);
                    RsaSecurityKey privateKey = new RsaSecurityKey(privateRsa);
                    signingCredentials = new SigningCredentials(privateKey, SecurityAlgorithms.RsaSha512);


                    JwtSecurityToken token = new JwtSecurityToken(
                        issuer: JwtSettings.Issuer,
                        audience: JwtSettings.Audience,
                        notBefore: DateTime.Now,
                        expires: DateTime.Now.AddMinutes(JwtSettings.ExpiricyInMinutes),
                        signingCredentials: signingCredentials,
                        claims: claims
                    );

                    return new JwtSecurityTokenHandler().WriteToken(token);
                }
            }
            else
                throw new Exception("nmjo bejelentkezes");
        }

        public async Task<string> RegisterAsync(string email, string password, string confirmPassword)
        {
            var user = new User { UserName = email, Email = email };
            var result = await UserManager.CreateAsync(user, password);

            if (result.Succeeded)
            {
                return await GetTokenAsync(email, password);
            }
            else throw new Exception("sikertelen resgisztralas");
        }
    }
}
