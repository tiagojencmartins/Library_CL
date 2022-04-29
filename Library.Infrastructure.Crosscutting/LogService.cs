using Library.Domain.Entities;
using Library.Infrastructure.Crosscutting.Abstract;

namespace Library.Infrastructure.Crosscutting
{
    public class LogService : ILogService
    {
        private readonly IRepositoryService _repositoryService;

        public LogService(IRepositoryService repositoryService)
        {
            _repositoryService = repositoryService;
        }

        public async Task LogAsync(Error error)
        {
            await _repositoryService.LogErrorAsync(error);
        }
    }
}
