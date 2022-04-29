using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Library.Domain.Entities
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; }

        public string Email { get; set; }

        public string Hash { get; set; }

        public string Salt { get; set; }

        public string Name { get; set; }

        public virtual UserRole Role { get; set; }
    }
}
