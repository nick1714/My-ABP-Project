using MyAbpProject.EntityFrameworkCore;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;

namespace MyAbpProject.DbMigrator;

[DependsOn(
    typeof(AbpAutofacModule),
    typeof(MyAbpProjectEntityFrameworkCoreModule),
    typeof(MyAbpProjectApplicationContractsModule)
)]
public class MyAbpProjectDbMigratorModule : AbpModule
{
}
