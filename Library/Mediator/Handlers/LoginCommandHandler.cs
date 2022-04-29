using Library.Application.Mediator.Commands;
using Library.Domain.Enums;
using Library.Domain.Models;
using Library.Infrastructure.Crosscutting.Abstract;
using Library.Infrastructure.Crosscutting.Helpers;
using MediatR;

namespace Library.Application.Mediator.Handlers
{
    public class LoginCommandHandler : IRequestHandler<LoginCommand, CommandResult>
    {
        private readonly IRepositoryService _repositoryService;
        private readonly IAuthService _authorizationService;

        public LoginCommandHandler(
            IRepositoryService repositoryService,
            IAuthService authorizationService)
        {
            _repositoryService = repositoryService;
            _authorizationService = authorizationService;
        }

        public async Task<CommandResult> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var user = await _repositoryService.GetUserByEmailAsync(request.Email);

            if (user == null)
            {
                return new(StatusEnum.NotFound, "No user found");
            }

            var password = HashSaltHelper.Salt(request.Password, user.Salt);

            if (password.Hash != user.Hash)
            {
                return new(StatusEnum.NotFound, "No user found");
            }

            return new(
                StatusEnum.Ok,
                _authorizationService.GenerateToken(user.Email, user.Role.Role));
        }
    }
}
