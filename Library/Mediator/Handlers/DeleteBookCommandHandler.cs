using Library.Application.Mediator.Commands;
using Library.Domain.Models;
using Library.Infrastructure.Crosscutting.Abstract;
using MediatR;

namespace Library.Application.Mediator.Handlers
{
    public class DeleteBookCommandHandler : IRequestHandler<DeleteBookCommand, CommandResult>
    {
        private readonly IRepositoryService _repositoryService;

        public DeleteBookCommandHandler(IRepositoryService repositoryService)
        {
            _repositoryService = repositoryService;
        }

        public Task<CommandResult> Handle(DeleteBookCommand request, CancellationToken cancellationToken)
        {
            return _repositoryService.DeleteBookAsync(request.BookId);
        }
    }
}
