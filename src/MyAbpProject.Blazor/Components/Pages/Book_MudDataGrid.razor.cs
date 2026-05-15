using Blazorise;
using Microsoft.AspNetCore.Authorization;
using MudBlazor;
using MyAbpProject.Books;
using MyAbpProject.Localization;
using MyAbpProject.Permissions;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace MyAbpProject.Blazor.Components.Pages
{
    public partial class Book_MudDataGrid
    {

        private MudDataGrid<BookDto> _dataGrid;

        // Biến lưu trữ quyền để ẩn/hiện nút tương ứng
        private bool HasCreatePermission;
        private bool HasUpdatePermission;
        private bool HasDeletePermission;

        public Book_MudDataGrid()
        {
            LocalizationResource = typeof(MyAbpProjectResource);
        }

        protected override async Task OnInitializedAsync()
        {
            // Kiểm tra quyền của user (Thay thế bằng tên Permission đúng của bạn)
            HasCreatePermission = await AuthorizationService.IsGrantedAsync(MyAbpProjectPermissions.Books.Create);
            HasUpdatePermission = await AuthorizationService.IsGrantedAsync(MyAbpProjectPermissions.Books.Edit);
            HasDeletePermission = await AuthorizationService.IsGrantedAsync(MyAbpProjectPermissions.Books.Delete);
        }

        // Đổi kiểu trả về và tham số đầu vào từ TableState sang GridState
        private async Task<MudBlazor.GridData<BookDto>> ServerReload(MudBlazor.GridState<BookDto> state, CancellationToken cancellationToken)
        {
            string sortQuery = null;

            // DataGrid quản lý sort dưới dạng danh sách, lấy phần tử đầu tiên nếu có
            var sortDefinition = state.SortDefinitions.FirstOrDefault();
            if (sortDefinition != null)
            {
                sortQuery = sortDefinition.Descending
                    ? $"{sortDefinition.SortBy} DESC"
                    : $"{sortDefinition.SortBy} ASC";
            }

            var result = await BookAppService.GetListAsync(new PagedAndSortedResultRequestDto
            {
                MaxResultCount = state.PageSize,
                SkipCount = state.Page * state.PageSize,
                Sorting = sortQuery
            });

            // Đổi TableData thành GridData
            return new MudBlazor.GridData<BookDto>() { TotalItems = (int)result.TotalCount, Items = result.Items };
        }

        // --- CÁC HÀM XỬ LÝ SỰ KIỆN NÚT BẤM ---
        private async Task OpenCreateDialogAsync()
        {
            var options = new DialogOptions
            {
                CloseOnEscapeKey = true,
                BackdropClick = true,
                MaxWidth = MaxWidth.Small,
                FullWidth = true
            };

            var dialog = await DialogService.ShowAsync<BookCreateDialog>("Thêm sách mới", options);
            var result = await dialog.Result;

            if (!result.Canceled)
            {
                // Đổi _table thành _dataGrid
                await _dataGrid.ReloadServerData();
            }
        }

        private async Task OpenEditDialogAsync(BookDto book)
        {
            var options = new DialogOptions
            {
                CloseOnEscapeKey = true,
                BackdropClick = true,
                MaxWidth = MaxWidth.Small,
                FullWidth = true
            };
            var parameters = new DialogParameters { ["Id"] = book.Id };

            var dialog = await DialogService.ShowAsync<BookEditDialog>("Cập nhật sách", parameters);
            var result = await dialog.Result;

            if (!result.Canceled)
            {
                // Đổi _table thành _dataGrid
                await _dataGrid.ReloadServerData();
            }
        }

        private async Task DeleteBookAsync(BookDto book)
        {
            bool? result = await DialogService.ShowMessageBoxAsync(
                "Xác nhận xóa",
                $"Bạn có chắc chắn muốn xóa sách '{book.Name}' không? Hành động này không thể hoàn tác.",
                yesText: "Xóa", cancelText: "Hủy");

            if (result == true)
            {
                await BookAppService.DeleteAsync(book.Id);
                Snackbar.Add("Đã xóa sách thành công", MudBlazor.Severity.Success);

                // Đổi _table thành _dataGrid
                await _dataGrid.ReloadServerData();
            }
        }
    }
}

