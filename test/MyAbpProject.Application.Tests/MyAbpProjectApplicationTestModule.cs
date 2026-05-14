using Volo.Abp.Modularity;

namespace MyAbpProject;

[DependsOn(
    typeof(MyAbpProjectApplicationModule),
    typeof(MyAbpProjectDomainTestModule)
)]
public class MyAbpProjectApplicationTestModule : AbpModule
{

}
