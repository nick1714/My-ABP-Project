using Volo.Abp.Modularity;

namespace MyAbpProject;

/* Inherit from this class for your domain layer tests. */
public abstract class MyAbpProjectDomainTestBase<TStartupModule> : MyAbpProjectTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
