using System.Threading.Tasks;

namespace MyAbpProject.Data;

public interface IMyAbpProjectDbSchemaMigrator
{
    Task MigrateAsync();
}
