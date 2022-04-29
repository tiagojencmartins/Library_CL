using Library.Domain.Entities;
using Library.Domain.Enums;
using Library.Domain.Models;
using Library.Infrastructure.Crosscutting.Abstract;
using Library.Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;

namespace Library.Infrastructure
{
    public class RepositoryService : IRepositoryService
    {
        private readonly LibraryContext _libraryContext;

        public RepositoryService(LibraryContext libraryContext)
        {
            _libraryContext = libraryContext;
        }

        public async Task<CommandResult> DeleteBookAsync(int id)
        {
            var book = await _libraryContext.Book.FirstOrDefaultAsync(book => book.BookId == id);

            if (book == null)
            {
                return new(StatusEnum.NotFound, "Book not found");
            }

            _libraryContext.Book.Remove(book);
            var nRows = await _libraryContext.SaveChangesAsync();

            return nRows == 1
                ? new(StatusEnum.Ok, string.Empty)
                : new(StatusEnum.NotModified, "Not modified");
        }

        public async Task<IEnumerable<Book>> GetAllBooksAsync()
        {
            return await _libraryContext
                .Book
                .Include(book => book.Authors)
                .ThenInclude(author => author.Author)
                .Distinct()
                .ToListAsync();
        }

        public async Task<IEnumerable<Author>> GetAuthorsByIdAsync(IEnumerable<int> authorIds)
        {
            return await _libraryContext
                .Authors
                .Where(author => authorIds.Contains(author.AuthorId))
                .ToListAsync();
        }

        public async Task<User?> GetUserByEmailAsync(string email)
        {
            return await _libraryContext
                .Users
                .Include(user => user.Role)
                .FirstOrDefaultAsync(token => token.Email.ToLowerInvariant() == email.ToLowerInvariant());
        }

        public async Task<bool> LogErrorAsync(Error error)
        {
            _libraryContext.Errors.Add(error);

            return await _libraryContext.SaveChangesAsync() > 0;
        }

        public async Task<IEnumerable<Book>> SearchBooksAsync(string title, string authorName)
        {
            var hasTitle = !string.IsNullOrWhiteSpace(title);
            var hasAuthor = !string.IsNullOrWhiteSpace(authorName);

            if (!hasTitle && !hasAuthor)
            {
                return await GetAllBooksAsync();
            }

            return await _libraryContext
                 .Book
                 .Include(book => book.Authors)
                 .ThenInclude(author => author.Author)
                 .Where(book =>
                    (hasTitle ? book.Title.ToLowerInvariant().Contains(title.ToLowerInvariant()) : true)
                    && (hasAuthor ? book.Authors.Any(author => author.Author.Name.ToLowerInvariant().Contains(authorName.ToLowerInvariant())) : true)
                 )
                 .Distinct()
                 .ToListAsync();
        }

        public async Task<CommandResult> UpdateBookAsync(Book book, IEnumerable<int> authorIds)
        {
            var currentBook = await _libraryContext
                .Book
                .Include(book => book.Authors)
                .ThenInclude(author => author.Author)
                .FirstOrDefaultAsync(current => current.BookId == book.BookId);

            if (currentBook == null)
            {
                return new(StatusEnum.NotFound, "Book not found");
            }

            var duplicatedBook = await _libraryContext
                .Book
                .FirstOrDefaultAsync(current =>
                    current.BookId != book.BookId
                    && current.ISBN.ToLowerInvariant() == book.ISBN.ToLowerInvariant());

            if (duplicatedBook != null)
            {
                return new(StatusEnum.NotFound, "Book with given ISBN found");
            }

            var authors = await GetAuthorsByIdAsync(authorIds);

            if (authors?.Count() != authorIds.Count())
            {
                return new(StatusEnum.NotFound, "Invalid author list");
            }

            currentBook.Authors.Clear();

            currentBook.Authors.AddRange(authors.Select(author => new AuthorBooks
            {
                AuthorId = author.AuthorId,
                BookId = book.BookId
            }).ToList());

            currentBook.BookId = book.BookId;
            currentBook.Description = book.Description;
            currentBook.NumberOfPages = book.NumberOfPages;
            currentBook.Title = book.Title;
            currentBook.ISBN = book.ISBN;

            _libraryContext.Book.Update(currentBook);

            var nRows = await _libraryContext.SaveChangesAsync();

            return nRows > 0
                ? new(StatusEnum.Ok, string.Empty)
                : new(StatusEnum.NotModified, "Not modified");
        }

        public async Task<CommandResult> CreateBookAsync(Book book, IEnumerable<int> authorIds)
        {
            var currentBook = await _libraryContext
                .Book
                .FirstOrDefaultAsync(current => current.ISBN.ToLowerInvariant() == book.ISBN.ToLowerInvariant());

            if (currentBook != null)
            {
                return new(StatusEnum.NotFound, "Book with given ISBN found");
            }

            var authors = await GetAuthorsByIdAsync(authorIds);

            if (authors?.Count() != authorIds.Count())
            {
                return new(StatusEnum.NotFound, "Invalid author list");
            }


            book.Authors.AddRange(authors.Select(author => new AuthorBooks
            {
                AuthorId = author.AuthorId,
                BookId = book.BookId
            }).ToList());

            await _libraryContext.Book.AddAsync(book);

            var nRows = await _libraryContext.SaveChangesAsync();

            return nRows > 0
                ? new(StatusEnum.Ok, book.BookId.ToString())
                : new(StatusEnum.NotModified, "Not modified");
        }
    }
}
