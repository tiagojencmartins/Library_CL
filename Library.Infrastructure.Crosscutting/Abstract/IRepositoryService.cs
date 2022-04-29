using Library.Domain.Entities;
using Library.Domain.Models;

namespace Library.Infrastructure.Crosscutting.Abstract
{
    public interface IRepositoryService
    {
        public Task<IEnumerable<Book>> GetAllBooksAsync();

        public Task<CommandResult> DeleteBookAsync(int id);

        public Task<CommandResult> UpdateBookAsync(Book book, IEnumerable<int> authorIds);

        public Task<CommandResult> CreateBookAsync(Book book, IEnumerable<int> authorIds);

        public Task<IEnumerable<Author>> GetAuthorsByIdAsync(IEnumerable<int> authorIds);

        public Task<IEnumerable<Book>> SearchBooksAsync(string title, string author);

        public Task<User?> GetUserByEmailAsync(string email);

        public Task<bool> LogErrorAsync(Error error);
    }
}
