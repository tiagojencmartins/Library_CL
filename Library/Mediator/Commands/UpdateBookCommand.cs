using Library.Application.Models.In;
using Library.Domain.Models;
using MediatR;

namespace Library.Application.Mediator.Commands
{
    public class UpdateBookCommand : IRequest<CommandResult>
    {
        public int BookId { get; }

        public BookModel Book { get; }

        public UpdateBookCommand(int bookId, BookModel book)
        {
            BookId = bookId;
            Book = book;
        }
    }
}
