using Icarus.Model.User;

namespace Icarus.API.Infrastructure.DistCache
{
    public interface IDistCache
    {
        public void SetCache(LoginViewModel loginUser);
        public UserViewModel GetCurrentUser();
    }
}
