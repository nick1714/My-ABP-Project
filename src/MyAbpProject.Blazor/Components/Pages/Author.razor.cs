using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using MyAbpProject.Authors;
using MyAbpProject.Permissions;
using System;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.ObjectMapping;

namespace MyAbpProject.Blazor.Components.Pages;

public partial class Author
{
    private MudTable<AuthorDto>? table;
    [Inject]
    protected IDialogService? DialogService { get; set; }
    private int PageSize { get; set; } = LimitedResultRequestDto.DefaultMaxResultCount;
    private int CurrentPage { get; set; }
    private string? CurrentSorting { get; set; }

    private bool CanCreateAuthor { get; set; }
    private bool CanEditAuthor { get; set; }
    private bool CanDeleteAuthor { get; set; }

    // Khởi tạo luôn Dto để tránh NullReferenceException lúc Component vừa render
    private CreateAuthorDto NewAuthor { get; set; } = new();
    private Guid EditingAuthorId { get; set; }
    private UpdateAuthorDto EditingAuthor { get; set; } = new();

    private bool isCreateModalOpen;
    private bool isEditModalOpen;
    private DialogOptions dialogOptions = new() { MaxWidth = MaxWidth.Small, FullWidth = true };

    private DateTime? createBirthDate
    {
        get => NewAuthor.BirthDate;
        set => NewAuthor.BirthDate = value ?? DateTime.Now;
    }

    private DateTime? editBirthDate
    {
        get => EditingAuthor.BirthDate;
        set => EditingAuthor.BirthDate = value ?? DateTime.Now;
    }

    protected override async Task OnInitializedAsync()
    {
        await SetPermissionsAsync();
    }

    private async Task SetPermissionsAsync()
    {
        CanCreateAuthor = await AuthorizationService.IsGrantedAsync(MyAbpProjectPermissions.Authors.Create);
        CanEditAuthor = await AuthorizationService.IsGrantedAsync(MyAbpProjectPermissions.Authors.Edit);
        CanDeleteAuthor = await AuthorizationService.IsGrantedAsync(MyAbpProjectPermissions.Authors.Delete);
    }

    private async Task<TableData<AuthorDto>> ServerReload(TableState state, CancellationToken cancellationToken)
    {
        // Fix lỗi dư khoảng trắng ở cuối chuỗi Sort
        CurrentSorting = string.IsNullOrEmpty(state.SortLabel)
            ? null
            : $"{state.SortLabel} {(state.SortDirection == SortDirection.Descending ? "DESC" : "")}".TrimEnd();

        CurrentPage = state.Page;
        PageSize = state.PageSize;

        var result = await AuthorAppService.GetListAsync(new GetAuthorListDto
        {
            MaxResultCount = PageSize,
            SkipCount = CurrentPage * PageSize,
            Sorting = CurrentSorting
        });

        return new TableData<AuthorDto>() { TotalItems = (int)result.TotalCount, Items = result.Items };
    }

    private void OpenCreateAuthorModal()
    {
        NewAuthor = new CreateAuthorDto();
        isCreateModalOpen = true;
    }

    private void CloseCreateAuthorModal()
    {
        isCreateModalOpen = false;
    }

    private void OpenEditAuthorModal(AuthorDto author)
    {
        EditingAuthorId = author.Id;
        EditingAuthor = ObjectMapper.Map<AuthorDto, UpdateAuthorDto>(author);
        isEditModalOpen = true;
    }

    private void CloseEditAuthorModal()
    {
        isEditModalOpen = false;
    }

    private async Task CreateAuthorAsync()
    {
        try
        {
            await AuthorAppService.CreateAsync(NewAuthor);
            isCreateModalOpen = false;

            if (table != null)
            {
                await table.ReloadServerData();
            }
        }
        catch (Exception ex)
        {
            await HandleErrorAsync(ex);
        }
    }

    private async Task UpdateAuthorAsync()
    {
        try
        {
            await AuthorAppService.UpdateAsync(EditingAuthorId, EditingAuthor);
            isEditModalOpen = false;

            if (table != null)
            {
                await table.ReloadServerData();
            }
        }
        catch (Exception ex)
        {
            await HandleErrorAsync(ex);
        }
    }

    private async Task DeleteAuthorAsync(AuthorDto author)
    {
        try
        {
            var confirmMessage = L["AuthorDeletionConfirmationMessage", author.Name];

            // Sử dụng DialogService của MudBlazor thay cho Message của ABP
            bool? result = await DialogService.ShowMessageBoxAsync(
                L["Delete"], // Tiêu đề hộp thoại
                confirmMessage,
                yesText: L["Yes"],
                cancelText: L["Cancel"]);

            // Nếu người dùng không bấm Yes thì hủy thao tác
            if (result != true)
            {
                return;
            }

            // Gọi API để xóa dữ liệu
            await AuthorAppService.DeleteAsync(author.Id);

            // Load lại bảng
            if (table != null)
            {
                await table.ReloadServerData();
            }
        }
        catch (Exception ex)
        {
            await HandleErrorAsync(ex);
        }
    }
}
