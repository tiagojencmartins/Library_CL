using Library.Application.Mediator.Commands;
using Library.Domain.Entities;
using Library.Infrastructure.Crosscutting.Abstract;
using MediatR;

namespace Library.Application.Mediator.Handlers
{
    public class GetAllBooksCommandHandler : IRequestHandler<GetAllBooksCommand, IEnumerable<Book>>
    {
        private readonly IRepositoryService _repositoryService;

        public GetAllBooksCommandHandler(IRepositoryService repositoryService)
        {
            _repositoryService = repositoryService;
        }

        public async Task<IEnumerable<Book>> Handle(GetAllBooksCommand request, CancellationToken cancellationToken)
        {
            return await _repositoryService.GetAllBooksAsync();
        }
    }
}
