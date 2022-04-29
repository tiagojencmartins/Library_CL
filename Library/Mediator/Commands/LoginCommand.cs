using Library.Domain.Models;
using MediatR;

namespace Library.Application.Mediator.Commands
{
    public class LoginCommand : IRequest<CommandResult>
    {
        public string Email { get; }

        public string Password { get; }

        public LoginCommand(string email, string password)
        {
            Email = email;
            Password = password;
        }
    }
}
