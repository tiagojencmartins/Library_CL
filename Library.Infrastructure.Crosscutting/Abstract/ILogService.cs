using Library.Domain.Entities;

namespace Library.Infrastructure.Crosscutting.Abstract
{
    public interface ILogService
    {
        Task LogAsync(Error error);
    }
}
