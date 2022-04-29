using Library.Domain.Entities;
using MediatR;

namespace Library.Application.Mediator.Commands
{
    public class GetAllBooksCommand : IRequest<IEnumerable<Book>>
    {
    }
}
