using MyAbpProject.Localization;
using Volo.Abp.AspNetCore.Components;

namespace MyAbpProject.Blazor;

public abstract class MyAbpProjectComponentBase : AbpComponentBase
{
    protected MyAbpProjectComponentBase()
    {
        LocalizationResource = typeof(MyAbpProjectResource);
    }
}
