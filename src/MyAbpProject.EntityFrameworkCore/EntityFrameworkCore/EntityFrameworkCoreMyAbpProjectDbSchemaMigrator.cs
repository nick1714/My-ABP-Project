using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MyAbpProject.Data;
using Volo.Abp.DependencyInjection;

namespace MyAbpProject.EntityFrameworkCore;

public class EntityFrameworkCoreMyAbpProjectDbSchemaMigrator
    : IMyAbpProjectDbSchemaMigrator, ITransientDependency
{
    private readonly IServiceProvider _serviceProvider;

    public EntityFrameworkCoreMyAbpProjectDbSchemaMigrator(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task MigrateAsync()
    {
        /* We intentionally resolving the MyAbpProjectDbContext
         * from IServiceProvider (instead of directly injecting it)
         * to properly get the connection string of the current tenant in the
         * current scope.
         */

        await _serviceProvider
            .GetRequiredService<MyAbpProjectDbContext>()
            .Database
            .MigrateAsync();
    }
}
