using AnalystPortal.API.Models.Domain;
using Microsoft.AspNetCore.Identity;

namespace AnalystPortal.API.Repositories.Interface
{
    public interface ITokenRepository
    {
        string CreateJWtToken(User user, List<string> roles);
    }
}
