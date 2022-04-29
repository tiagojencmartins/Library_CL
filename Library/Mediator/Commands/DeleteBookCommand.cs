using Library.Domain.Models;
using MediatR;

namespace Library.Application.Mediator.Commands
{
    public class DeleteBookCommand : IRequest<CommandResult>
    {
        public int BookId { get; }

        public DeleteBookCommand(int bookId)
        {
            BookId = bookId;
        }
    }
}
