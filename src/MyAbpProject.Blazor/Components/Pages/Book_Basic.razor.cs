using MudBlazor;
using MyAbpProject.Blazor.Components.Pages;
using MyAbpProject.Books;
using MyAbpProject.Localization;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace MyAbpProject.Blazor.Components.Pages;

public partial class Book_Basic : MyAbpProjectComponentBase
{
    public Book_Basic()
    {
        LocalizationResource = typeof(MyAbpProjectResource);
    }

    // Hàm này sẽ tự động được gọi bởi MudTable mỗi khi đổi trang, đổi pageSize hoặc Sort
    private async Task<TableData<BookDto>> ServerReload(TableState state, CancellationToken cancellationToken)
    {
        // Gọi thẳng AppService của ABP để lấy dữ liệu
        var result = await BookAppService.GetListAsync(new PagedAndSortedResultRequestDto
        {
            MaxResultCount = state.PageSize,
            SkipCount = state.Page * state.PageSize,
            Sorting = state.SortLabel != null ? $"{state.SortLabel} {(state.SortDirection == MudBlazor.SortDirection.Descending ? "DESC" : "ASC")}" : null
        });

        // Trả về định dạng mà MudTable hiểu được
        return new TableData<BookDto>()
        {
            TotalItems = (int)result.TotalCount,
            Items = result.Items
        };
    }
}