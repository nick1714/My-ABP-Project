using MyAbpProject.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace MyAbpProject.Controllers;

/* Inherit your controllers from this class.
 */
public abstract class MyAbpProjectController : AbpControllerBase
{
    protected MyAbpProjectController()
    {
        LocalizationResource = typeof(MyAbpProjectResource);
    }
}
