using Library.Domain.Enums;

namespace Library.Infrastructure.Crosscutting.Abstract
{
    public interface IAuthService
    {
        public string GenerateToken(string email, string role);

        public bool ValidateToken(string token);
    }
}
