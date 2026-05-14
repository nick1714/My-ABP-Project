using MyAbpProject.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;
using Volo.Abp.MultiTenancy;

namespace MyAbpProject.Permissions;

public class MyAbpProjectPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var bookStoreGroup = context.AddGroup(MyAbpProjectPermissions.GroupName, L("Permission:BookStore"));

        ////Dashboard permissions
        //bookStoreGroup.AddPermission(MyAbpProjectPermissions.Dashboard.Host, L("Permission:Dashboard"), MultiTenancySides.Host);
        //bookStoreGroup.AddPermission(MyAbpProjectPermissions.Dashboard.Tenant, L("Permission:Dashboard"), MultiTenancySides.Tenant);
        //Books permissions
        var booksPermission = bookStoreGroup.AddPermission(MyAbpProjectPermissions.Books.Default, L("Permission:Books"));
        booksPermission.AddChild(MyAbpProjectPermissions.Books.Create, L("Permission:Books.Create"));
        booksPermission.AddChild(MyAbpProjectPermissions.Books.Edit, L("Permission:Books.Edit"));
        booksPermission.AddChild(MyAbpProjectPermissions.Books.Delete, L("Permission:Books.Delete"));

    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<MyAbpProjectResource>(name);
    }
}
