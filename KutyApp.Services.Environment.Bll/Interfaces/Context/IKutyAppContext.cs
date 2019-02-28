using KutyApp.Services.Environment.Bll.Dtos;
using System.Threading.Tasks;

namespace KutyApp.Services.Environment.Bll.Interfaces.Context
{
    public interface IKutyAppContext
    {
        CurrentUserDto CurrentUser { get; set; }
        string IpAddress { get; set; }
        Task LoadCurrentUserAsync(string email);
    }
}
