using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace MyAbpProject.Data;

/* This is used if database provider does't define
 * IMyAbpProjectDbSchemaMigrator implementation.
 */
public class NullMyAbpProjectDbSchemaMigrator : IMyAbpProjectDbSchemaMigrator, ITransientDependency
{
    public Task MigrateAsync()
    {
        return Task.CompletedTask;
    }
}
