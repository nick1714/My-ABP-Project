using System.Threading.Tasks;
using MyAbpProject.Localization;
using MyAbpProject.Permissions;
using MyAbpProject.MultiTenancy;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.UI.Navigation;
using Volo.Abp.SettingManagement.Blazor.Menus;
using Volo.Abp.TenantManagement.Blazor.Navigation;
using Volo.Abp.Identity.Blazor;

namespace MyAbpProject.Blazor.Menus;

public class MyAbpProjectMenuContributor : IMenuContributor
{
    public async Task ConfigureMenuAsync(MenuConfigurationContext context)
    {
        if (context.Menu.Name == StandardMenus.Main)
        {
            await ConfigureMainMenuAsync(context);
        }
    }

    private Task ConfigureMainMenuAsync(MenuConfigurationContext context)
    {
        var l = context.GetLocalizer<MyAbpProjectResource>();
        
        context.Menu.Items.Insert(
            0,
            new ApplicationMenuItem(
                MyAbpProjectMenus.Home,
                l["Menu:Home"],
                "/",
                icon: "fas fa-home",
                order: 1
            )
        );
        //context.Menu.AddItem(
        //    new ApplicationMenuItem(
        //        "BooksStore",
        //        l["Menu:BookStore"],
        //        icon: "fa fa-book"
        //    ).AddItem(
        //        new ApplicationMenuItem(
        //            "BooksStore.Books",
        //            l["Menu:Books_Basic"],
        //            url: "/books_basic"
        //        )
        //    ).AddItem(
        //        new ApplicationMenuItem(
        //            "BooksStore.Books",
        //            l["Menu:Books_MudDataGrid"],
        //            url: "/books_muddatagrid"
        //        )
        //    ).AddItem(
        //        new ApplicationMenuItem(
        //            "BooksStore.Books_MudTable",
        //            l["Menu:Books"],
        //            url: "/books_mudtable"
        //        )
        //    )
        //);

        var bookStoreMenu = new ApplicationMenuItem(
            "BooksStore",
            l["Menu:BookStore"],
            icon: "fa fa-book"
        );
        context.Menu.AddItem(bookStoreMenu);

        bookStoreMenu.AddItem(new ApplicationMenuItem(
            "BooksStore.Books",
            l["Menu:Books_Basic"],
            url: "/books_basic"
        ).RequirePermissions(MyAbpProjectPermissions.Books.Default));

        bookStoreMenu.AddItem(new ApplicationMenuItem(
            "BooksStore.Books",
            l["Menu:Books_MudDataGrid"],
            url: "/books_muddatagrid"
        ).RequirePermissions(MyAbpProjectPermissions.Books.Default));

        bookStoreMenu.AddItem(new ApplicationMenuItem(
            "BooksStore.Books",
            l["Menu:Books_MudTable"],
            url: "/books_mudtable"
        ).RequirePermissions(MyAbpProjectPermissions.Books.Default));

        context.Menu.AddItem(new ApplicationMenuItem(
            "BooksStore.Authors",
            l["Menu:Authors"],
            url: "/author"
        ).RequirePermissions(MyAbpProjectPermissions.Books.Default));

        //Administration
        var administration = context.Menu.GetAdministration();
        administration.Order = 6;
    
        if (MultiTenancyConsts.IsEnabled)
        {
            administration.SetSubItemOrder(TenantManagementMenuNames.GroupName, 1);
        }
        else
        {
            administration.TryRemoveMenuItem(TenantManagementMenuNames.GroupName);
        }

        administration.SetSubItemOrder(IdentityMenuNames.GroupName, 2);
        administration.SetSubItemOrder(SettingManagementMenus.GroupName, 3);

        return Task.CompletedTask;
    }
}
