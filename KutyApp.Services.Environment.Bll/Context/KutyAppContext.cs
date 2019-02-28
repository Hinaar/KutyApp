using System.Linq;
using System.Threading.Tasks;
using KutyApp.Services.Environment.Bll.Dtos;
using KutyApp.Services.Environment.Bll.Entities;
using KutyApp.Services.Environment.Bll.Interfaces.Context;
using Microsoft.EntityFrameworkCore;

namespace KutyApp.Services.Environment.Bll.Context
{
    public class KutyAppContext : IKutyAppContext
    {
        public CurrentUserDto CurrentUser { get; set ; }
        public string IpAddress { get; set; }

        private KutyAppServiceDbContext DbContext { get; }

        public KutyAppContext(KutyAppServiceDbContext dbContext)
        {
            DbContext = dbContext;
        }

        public async Task LoadCurrentUserAsync(string email)
        {
            var user = await DbContext.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Email == email);
            if (user != null)
                CurrentUser = new CurrentUserDto { Email = user.Email, Id = user.Id, Name = user.UserName };
        }
    }
}
