using Microsoft.AspNetCore.Authorization;
using MudBlazor;
using MyAbpProject.Books;
using MyAbpProject.Localization;
using MyAbpProject.Permissions;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace MyAbpProject.Blazor.Components.Pages
{
    public partial class Book_MudTable
    {

        private MudTable<BookDto> _table;
        private MudDataGrid<BookDto> _dataGrid;


        // Biến lưu trữ quyền để ẩn/hiện nút tương ứng
        private bool HasCreatePermission;
        private bool HasUpdatePermission;
        private bool HasDeletePermission;

        public Book_MudTable()
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

        private async Task<MudBlazor.TableData<BookDto>> ServerReload(MudBlazor.TableState state, System.Threading.CancellationToken token)
        {
            string sortQuery = null;
            if (!string.IsNullOrEmpty(state.SortLabel))
            {
                sortQuery = state.SortDirection == MudBlazor.SortDirection.Descending
                    ? $"{state.SortLabel} DESC"
                    : $"{state.SortLabel} ASC";
            }

            var result = await BookAppService.GetListAsync(new PagedAndSortedResultRequestDto
            {
                MaxResultCount = state.PageSize,
                SkipCount = state.Page * state.PageSize,
                Sorting = sortQuery
            });

            return new MudBlazor.TableData<BookDto>() { TotalItems = (int)result.TotalCount, Items = result.Items };
        }

        // --- CÁC HÀM XỬ LÝ SỰ KIỆN NÚT BẤM ---

        // private async Task OpenCreateDialogAsync()
        // {
        //     TODO: Mở Hộp thoại (Dialog) chứa Form thêm mới sách
        //     Sau khi thêm thành công, gọi _table.ReloadServerData();
        //     Snackbar.Add("Chức năng thêm sách đang được xây dựng", MudBlazor.Severity.Info);
        // }

        private async Task OpenCreateDialogAsync()
        {
            // Cấu hình giao diện cho Hộp thoại (không cho bấm ra ngoài để đóng, độ rộng vừa phải)
            var options = new DialogOptions
            {
                CloseOnEscapeKey = true,
                BackdropClick = true,
                MaxWidth = MaxWidth.Small,
                FullWidth = true
            };

            // Gọi hiển thị Component BookCreateDialog mà ta vừa tạo
            var dialog = await DialogService.ShowAsync<BookCreateDialog>("Thêm sách mới", options);

            // Chờ người dùng thao tác xong (bấm Lưu hoặc Hủy)
            var result = await dialog.Result;

            // Nếu người dùng bấm "Lưu" (Dialog trả về trạng thái không bị Cancel)
            if (!result.Canceled)
            {
                // Tải lại bảng dữ liệu để hiển thị cuốn sách vừa thêm
                await _table.ReloadServerData();
            }
        }

        private async Task OpenEditDialogAsync(BookDto book)
        {
            // TODO: Mở Hộp thoại (Dialog) chứa Form cập nhật sách truyền vào book.Id
            // Sau khi sửa thành công, gọi _table.ReloadServerData();
            Snackbar.Add($"Đang mở form sửa sách: {book.Name}", MudBlazor.Severity.Info);
            var options = new DialogOptions
            {
                CloseOnEscapeKey = true,
                BackdropClick = true,
                MaxWidth = MaxWidth.Small,
                FullWidth = true
            };
            var parameters = new DialogParameters { ["Id"] = book.Id };
            // Gọi hiển thị Component BookEditDialog mà ta vừa tạo
            var dialog = await DialogService.ShowAsync<BookEditDialog>("Cập nhật sách", parameters);

            var result = await dialog.Result;

            if (!result.Canceled)
            {
                await _table.ReloadServerData();
            }
        }

        private async Task DeleteBookAsync(BookDto book)
        {
            // Mở popup xác nhận xóa của MudBlazor
            bool? result = await DialogService.ShowMessageBoxAsync(
                "Xác nhận xóa",
                $"Bạn có chắc chắn muốn xóa sách '{book.Name}' không? Hành động này không thể hoàn tác.",
                yesText: "Xóa", cancelText: "Hủy");

            if (result == true)
            {
                // Gọi API xóa của ABP
                await BookAppService.DeleteAsync(book.Id);

                // Hiện thông báo góc màn hình
                Snackbar.Add("Đã xóa sách thành công", MudBlazor.Severity.Success);

                // Tải lại dữ liệu bảng
                await _table.ReloadServerData();
            }
        }
    }
}

