namespace Library.Application.Models.Out
{
    public class BookViewModel
    {
        public string Title { get; set; }

        public IEnumerable<string> Authors { get; set; }
    }
}
