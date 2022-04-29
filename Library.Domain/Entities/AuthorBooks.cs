namespace Library.Domain.Entities
{
    public class AuthorBooks
    {
        public int AuthorId { get; set; }
        public virtual Author Author { get; set; }

        public int BookId { get; set; }
        public virtual Book Book { get; set; }
    }
}
