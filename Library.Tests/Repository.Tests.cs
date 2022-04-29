using Library.Domain.Enums;
using Library.Infrastructure;
using Library.Infrastructure.Crosscutting.Abstract;
using Library.Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace CrossCutting.Tests;

public class RepositoryTests
{
    private IServiceProvider _serviceProvider;

    [SetUp]
    public void Setup()
    {
        var services = new ServiceCollection();

        services.AddInfrastructureDependencies();

        _serviceProvider = services.BuildServiceProvider();
    }

    [Test]
    public void Repository_HasTwoUsers()
    {
        using var repository = _serviceProvider.GetService<LibraryContext>();

        Assert.AreEqual(repository!.Users.Count(), 2);
    }

    [Test]
    [TestCase("customer@library.com", UserRoleEnum.Customer)]
    [TestCase("librarian@library.com", UserRoleEnum.Librarian)]
    public void Repository_CustomerHasCustomerRole(string email, UserRoleEnum role)
    {
        using var repository = _serviceProvider.GetService<LibraryContext>();

        var customer = repository
            .Users
            .Include(user => user.Role)
            .FirstOrDefault(user => user.Email.Equals(email));

        Assert.AreEqual(customer!.Role.Role, role.ToString());
    }

    [Test]
    [TestCase("J.K. Rowling", 2)]
    [TestCase("J.R.R. Tolkien", 1)]
    public void Repository_CorrectNumberOfBooks(string authorName, int numberOfBooks)
    {
        using var repository = _serviceProvider.GetService<LibraryContext>();

        var booksCount = repository
            .Authors
            .Include(book => book.Books)
            .FirstOrDefault(author => author.Name.Equals(authorName))
            !.Books
            .Count;

        Assert.AreEqual(booksCount, numberOfBooks);
    }

    [Test]
    [TestCase("harry", "", 2)]
    [TestCase("harry", "row", 2)]
    [TestCase("", "tolkien", 1)]
    [TestCase("none", "none", 0)]
    public async Task Repository_SearchResults(string title, string author, int bookCount)
    {
        var repository = _serviceProvider.GetService<IRepositoryService>();

        var searchData = await repository!.SearchBooksAsync(title, author);

        Assert.AreEqual(searchData.Count(), bookCount);
    }
}