using MyAbpProject.Localization;
using Volo.Abp.Application.Services;

namespace MyAbpProject;

/* Inherit your application services from this class.
 */
public abstract class MyAbpProjectAppService : ApplicationService
{
    protected MyAbpProjectAppService()
    {
        LocalizationResource = typeof(MyAbpProjectResource);
    }
}
