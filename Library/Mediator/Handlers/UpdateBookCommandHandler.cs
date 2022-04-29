using Library.Application.Mediator.Commands;
using Library.Domain.Entities;
using Library.Domain.Models;
using Library.Infrastructure.Crosscutting.Abstract;
using MediatR;

namespace Library.Application.Mediator.Handlers
{
    public class UpdateBookCommandHandler : IRequestHandler<UpdateBookCommand, CommandResult>
    {
        private readonly IRepositoryService _repositoryService;

        public UpdateBookCommandHandler(IRepositoryService repositoryService)
        {
            _repositoryService = repositoryService;
        }

        public async Task<CommandResult> Handle(UpdateBookCommand request, CancellationToken cancellationToken)
        {
            var book = new Book
            {
                BookId = request.BookId,
                Description = request.Book.Description,
                NumberOfPages = request.Book.NumberOfPages,
                Title = request.Book.Title,
                ISBN = request.Book.ISBN
            };

            return await _repositoryService.UpdateBookAsync(book, request.Book.Authors);
        }
    }
}
