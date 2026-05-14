using MyAbpProject.Samples;
using Xunit;

namespace MyAbpProject.EntityFrameworkCore.Domains;

[Collection(MyAbpProjectTestConsts.CollectionDefinitionName)]
public class EfCoreSampleDomainTests : SampleDomainTests<MyAbpProjectEntityFrameworkCoreTestModule>
{

}
