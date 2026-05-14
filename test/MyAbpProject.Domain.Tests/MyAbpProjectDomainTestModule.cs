using Volo.Abp.Modularity;

namespace MyAbpProject;

[DependsOn(
    typeof(MyAbpProjectDomainModule),
    typeof(MyAbpProjectTestBaseModule)
)]
public class MyAbpProjectDomainTestModule : AbpModule
{

}
