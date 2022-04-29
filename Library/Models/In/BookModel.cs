using System.ComponentModel.DataAnnotations;

namespace Library.Application.Models.In
{
    public class BookModel
    {
        [Required]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public short NumberOfPages { get; set; }

        [Required]
        public List<int> Authors { get; set; }

        [Required]
        public string ISBN { get; set; }
    }
}
