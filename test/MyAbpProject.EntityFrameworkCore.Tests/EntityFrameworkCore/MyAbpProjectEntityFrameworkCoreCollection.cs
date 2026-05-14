using Xunit;

namespace MyAbpProject.EntityFrameworkCore;

[CollectionDefinition(MyAbpProjectTestConsts.CollectionDefinitionName)]
public class MyAbpProjectEntityFrameworkCoreCollection : ICollectionFixture<MyAbpProjectEntityFrameworkCoreFixture>
{

}
