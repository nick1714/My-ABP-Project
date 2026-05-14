using Volo.Abp.Modularity;

namespace MyAbpProject;

public abstract class MyAbpProjectApplicationTestBase<TStartupModule> : MyAbpProjectTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
