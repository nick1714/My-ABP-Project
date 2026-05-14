using Volo.Abp.Application.Dtos;

namespace MyAbpProject.Authors;

public class GetAuthorListDto : PagedAndSortedResultRequestDto
{
    public string? Filter { get; set; }
}
