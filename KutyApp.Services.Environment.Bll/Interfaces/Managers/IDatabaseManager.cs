using KutyApp.Services.Environment.Bll.Dtos;
using System.Threading.Tasks;

namespace KutyApp.Services.Environment.Bll.Interfaces
{
    public interface IDatabaseManager
    {
        Task TruncateDatabaseAsync();
        Task SeedDatabaseAsync();
        Task<DbVersionDto> GetDBVersionAsync();
    }
}
