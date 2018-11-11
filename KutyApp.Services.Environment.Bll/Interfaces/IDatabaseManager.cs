using System.Threading.Tasks;

namespace KutyApp.Services.Environment.Bll.Interfaces
{
    public interface IDatabaseManager
    {
        Task TruncateDatabaseAsync();
        Task SeedDatabaseAsync();
    }
}
