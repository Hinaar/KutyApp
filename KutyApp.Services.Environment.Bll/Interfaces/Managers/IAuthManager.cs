using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace KutyApp.Services.Environment.Bll.Interfaces.Managers
{
    public interface IAuthManager
    {
        Task<string> GetTokenAsync(string email, string password);
        Task<string> RegisterAsync(string email, string password, string confirmPassword);
    }
}
