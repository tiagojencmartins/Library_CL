using Library.Domain.Enums;

namespace Library.Domain.Models
{
    public class CommandResult
    {
        public StatusEnum Status { get; }

        public string Description { get; }

        public CommandResult(StatusEnum status, string description)
        {
            Status = status;
            Description = description;
        }
    }
}
