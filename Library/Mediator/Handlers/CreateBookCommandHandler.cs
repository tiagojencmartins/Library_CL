using Library.Application.Mediator.Commands;
using Library.Domain.Entities;
using Library.Domain.Models;
using Library.Infrastructure.Crosscutting.Abstract;
using MediatR;

namespace Library.Application.Mediator.Handlers
{
    public class CreateBookCommandHandler : IRequestHandler<CreateBookCommand, CommandResult>
    {
        private readonly IRepositoryService _repositoryService;

        public CreateBookCommandHandler(IRepositoryService repositoryService)
        {
            _repositoryService = repositoryService;
        }

        public async Task<CommandResult> Handle(CreateBookCommand request, CancellationToken cancellationToken)
        {
            var book = new Book
            {
                Description = request.Book.Description,
                NumberOfPages = request.Book.NumberOfPages,
                Title = request.Book.Title,
                ISBN = request.Book.ISBN
            };

            return await _repositoryService.CreateBookAsync(book, request.Book.Authors);
        }
    }
}
