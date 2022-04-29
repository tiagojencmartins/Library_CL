using Library.Application.Models.In;
using Library.Domain.Models;
using MediatR;

namespace Library.Application.Mediator.Commands
{
    public class CreateBookCommand : IRequest<CommandResult>
    {
        public BookModel Book { get; }

        public CreateBookCommand(BookModel book)
        {
            Book = book;
        }
    }
}
