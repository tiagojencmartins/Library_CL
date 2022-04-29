using Library.Domain.Entities;
using Library.Domain.Enums;
using Library.Infrastructure.Crosscutting.Helpers;
using Microsoft.EntityFrameworkCore;

namespace Library.Infrastructure.Repository
{
    public class LibraryContext : DbContext
    {
        public DbSet<Book> Book { get; set; }

        public DbSet<Author> Authors { get; set; }

        public DbSet<AuthorBooks> AuthorBooks { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<UserRole> UserRoles { get; set; }

        public DbSet<Error> Errors { get; set; }


        public LibraryContext(DbContextOptions<LibraryContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            AddRoles(modelBuilder);
            AddUsers(modelBuilder);
            AddAuthors(modelBuilder);
            AddBooks(modelBuilder);
        }

        private void AddRoles(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserRole>().HasData(
                new UserRole
                {
                    RoleId = 1,
                    Role = UserRoleEnum.Librarian.ToString()
                },
                new UserRole
                {
                    RoleId = 2,
                    Role = UserRoleEnum.Customer.ToString()
                });
        }

        private void AddUsers(ModelBuilder modelBuilder)
        {
            var librarianSalt = HashSaltHelper.Salt("librarianPassword");
            var customerSalt = HashSaltHelper.Salt("customerPassword");

            modelBuilder.Entity<User>()
                .HasOne(m => m.Role)
                 .WithOne()
                 .HasForeignKey<UserRole>(a => a.RoleId);

            modelBuilder.Entity<User>()
               .HasData(
                new
                {
                    UserId = 1,
                    Email = "librarian@library.com",
                    Hash = librarianSalt.Hash,
                    Salt = librarianSalt.Salt,
                    Name = "Librarian",
                    RoleId = 1
                },
                new
                {
                    UserId = 2,
                    Email = "customer@library.com",
                    Hash = customerSalt.Hash,
                    Salt = customerSalt.Salt,
                    Name = "Customer",
                    RoleId = 2
                });
        }

        private void AddAuthors(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Author>().HasData(
                new
                {
                    AuthorId = 1,
                    CreatedDate = DateTime.UtcNow,
                    Name = "J.K. Rowling"
                },
                new
                {
                    AuthorId = 2,
                    CreatedDate = DateTime.UtcNow,
                    Name = "J.R.R. Tolkien"
                });
        }

        private void AddBooks(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AuthorBooks>().HasKey(
                sc => new { sc.AuthorId, sc.BookId });

            modelBuilder.Entity<AuthorBooks>()
                .HasOne(sc => sc.Book)
                .WithMany(s => s.Authors)
                .HasForeignKey(sc => sc.BookId);


            modelBuilder.Entity<AuthorBooks>()
                .HasOne(sc => sc.Author)
                .WithMany(s => s.Books)
                .HasForeignKey(sc => sc.AuthorId);

            modelBuilder.Entity<Book>().HasData(
                new
                {
                    BookId = 1,
                    Title = "Harry Potter and the Sorcerers Stone",
                    Description = "Lorem Ipsum",
                    ISBN = "123-456-789",
                    NumberOfPages = (short)100
                },
                new
                {
                    BookId = 2,
                    Title = "Harry Potter and the Chamber of Secrets",
                    Description = "Lorem Ipsum",
                    ISBN = "123-789-456",
                    NumberOfPages = (short)200
                },
                new
                {
                    BookId = 3,
                    Title = "Lord of the Rings: Fellowship of the Ring",
                    Description = "Lorem Ipsum",
                    ISBN = "789-123-456",
                    NumberOfPages = (short)300
                });


            modelBuilder.Entity<AuthorBooks>().HasData(
                new AuthorBooks { AuthorId = 1, BookId = 1 },
                new AuthorBooks { AuthorId = 1, BookId = 2 },
                new AuthorBooks { AuthorId = 2, BookId = 3 });
        }
    }
}
