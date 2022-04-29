using Library.Domain.Entities;
using MediatR;

namespace Library.Application.Mediator.Commands
{
    public class SearchBookCommand : IRequest<IEnumerable<Book>>
    {
        public string Title { get; }
        public string Author { get; }

        public SearchBookCommand(string title, string author)
        {
            Title = title;
            Author = author;
        }
    }
}
