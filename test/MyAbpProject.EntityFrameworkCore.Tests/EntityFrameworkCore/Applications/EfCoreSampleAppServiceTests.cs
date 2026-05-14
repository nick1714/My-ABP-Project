using MyAbpProject.Samples;
using Xunit;

namespace MyAbpProject.EntityFrameworkCore.Applications;

[Collection(MyAbpProjectTestConsts.CollectionDefinitionName)]
public class EfCoreSampleAppServiceTests : SampleAppServiceTests<MyAbpProjectEntityFrameworkCoreTestModule>
{

}
