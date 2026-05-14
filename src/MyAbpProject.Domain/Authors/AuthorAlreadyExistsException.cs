using MyAbpProject;
using Volo.Abp;

namespace MyAbpProject.Authors;

public class AuthorAlreadyExistsException : BusinessException
{
    public AuthorAlreadyExistsException(string name)
        : base(MyAbpProjectDomainErrorCodes.AuthorAlreadyExists)
    {
        WithData("name", name);
    }
}
