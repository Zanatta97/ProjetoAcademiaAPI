using ProjetoAcademiaAPI.Application.Interfaces;
using System.Security.Claims;

namespace ProjetoAcademiaAPI.Api.Services
{
    public sealed class CurrentUserService(IHttpContextAccessor acessor) : ICurrentUser
    {
        public Guid UserId
        {
            get
            {
                var value = acessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier)
                    ?? throw new UnauthorizedAccessException("Usuário não autenticado.");
                return Guid.Parse(value);
            }
        }
    }
}
