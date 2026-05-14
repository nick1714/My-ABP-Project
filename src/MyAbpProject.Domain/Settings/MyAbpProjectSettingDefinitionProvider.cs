using Volo.Abp.Settings;

namespace MyAbpProject.Settings;

public class MyAbpProjectSettingDefinitionProvider : SettingDefinitionProvider
{
    public override void Define(ISettingDefinitionContext context)
    {
        //Define your own settings here. Example:
        //context.Add(new SettingDefinition(MyAbpProjectSettings.MySetting1));
    }
}
