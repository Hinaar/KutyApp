﻿using KutyApp.Services.Environment.Bll.Dtos;
using System.Threading.Tasks;

namespace KutyApp.Services.Environment.Bll.Interfaces
{
    public interface IAuthManager
    {
        Task<string> GetTokenAsync(LoginDto dto);
        Task<string> RegisterAsync(RegisterDto dto);
        Task<string> GetUserIdAsync(string userName);
    }
}
