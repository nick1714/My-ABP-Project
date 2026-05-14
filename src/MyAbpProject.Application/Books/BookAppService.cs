using MyAbpProject.Permissions;
using System;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace MyAbpProject.Books;

public class BookAppService :
    CrudAppService<
        Book, //The Book entity
        BookDto, //Used to show books
        Guid, //Primary key of the book entity
        PagedAndSortedResultRequestDto, //Used for paging/sorting
        CreateUpdateBookDto>, //Used to create/update a book
    IBookAppService //implement the IBookAppService
{
    public BookAppService(IRepository<Book, Guid> repository)
        : base(repository)
    {
        GetPolicyName = MyAbpProjectPermissions.Books.Default;
        GetListPolicyName = MyAbpProjectPermissions.Books.Default;
        CreatePolicyName = MyAbpProjectPermissions.Books.Create;
        UpdatePolicyName = MyAbpProjectPermissions.Books.Edit;
        DeletePolicyName = MyAbpProjectPermissions.Books.Delete;
    }
}
