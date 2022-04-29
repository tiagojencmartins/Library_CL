using Library.Application.Mediator.Commands;
using Library.Domain.Entities;
using Library.Infrastructure.Crosscutting.Abstract;
using MediatR;

namespace Library.Application.Mediator.Handlers
{
    public class SearchBookCommandHandler : IRequestHandler<SearchBookCommand, IEnumerable<Book>>
    {
        private readonly IRepositoryService _repositoryService;

        public SearchBookCommandHandler(IRepositoryService repositoryService)
        {
            _repositoryService = repositoryService;
        }

        public async Task<IEnumerable<Book>> Handle(SearchBookCommand request, CancellationToken cancellationToken)
        {
            return await _repositoryService.SearchBooksAsync(request.Title, request.Author);
        }
    }
}
