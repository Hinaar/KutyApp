using AutoMapper;
using KutyApp.Services.Environment.Api.Extensions;
using KutyApp.Services.Environment.Bll.Configuration;
using KutyApp.Services.Environment.Bll.Dtos;
using KutyApp.Services.Environment.Bll.Entities.Model;
using KutyApp.Services.Environment.Bll.Interfaces;
using KutyApp.Services.Environment.Bll.Resources;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace KutyApp.Services.Environment.Api.Managers
{
    public class AuthManager : IAuthManager
    {
        private SignInManager<User> SignInManager { get; }
        private UserManager<User> UserManager { get; }
        private JwtSettings JwtSettings { get; }
        private IMapper Mapper { get; }

        public AuthManager(SignInManager<User> signInManager, UserManager<User> userManager, IOptions<JwtSettings> jwtSettings, IMapper mapper)
        {
            SignInManager = signInManager;
            UserManager = userManager;
            JwtSettings = jwtSettings.Value;
            Mapper = mapper;
        }
        public async Task<string> GetTokenAsync(LoginDto dto)
        {
            var result = await SignInManager.PasswordSignInAsync(dto.Email, dto.Password, false, false);

            if (result.Succeeded)
            {
                //var user = await UserManager.FindByNameAsync(userName);
                var user = await UserManager.FindByEmailAsync(dto.Email);

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
                        expires: dto.RememberMe ? (DateTime?)null : DateTime.Now.AddMinutes(JwtSettings.ExpiricyInMinutes),
                        signingCredentials: signingCredentials,
                        claims: claims
                    );

                    return new JwtSecurityTokenHandler().WriteToken(token);
                }
            }
            else
                throw new Exception(ExceptionMessages.LoginError);
        }

        public async Task<string> RegisterAsync(RegisterDto dto)
        {
            if (dto.Password != dto.PasswordConfirm)
                throw new Exception(ExceptionMessages.ConfirmPassword);

            var user = new User { UserName = dto.Email, Email = dto.Email, PhoneNumber = dto.PhoneNumber };
            var result = await UserManager.CreateAsync(user, dto.Password);

            if (result.Succeeded)
            {
                return await GetTokenAsync(new LoginDto { Email = dto.Email, Password = dto.Password });
            }
            else throw new Exception(ExceptionMessages.RegisterError);
        }

        public async Task<string> GetUserIdAsync(string userName)
        {
            var user = await UserManager.FindByNameAsync(userName);
            if (user == null)
                throw new Exception(ExceptionMessages.NotFound);

            else return user.Id;
        }

        public async Task<UserDto> GetUserAsync(string name)
        {
            var user = await UserManager.FindByNameAsync(name);

            if (user == null)
                user = await UserManager.FindByEmailAsync(name);

            if (user == null)
                throw new Exception(ExceptionMessages.NotFound);

            return Mapper.Map<UserDto>(user);
        }
    }
}
