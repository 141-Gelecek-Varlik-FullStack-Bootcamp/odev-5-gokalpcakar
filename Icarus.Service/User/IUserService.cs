using Icarus.Model;
using Icarus.Model.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icarus.Service.User
{
    // UserService class'ında kullanılacak metotları burada tanımladık
    public interface IUserService
    {
        public General<UserViewModel> Login(LoginViewModel loginUser);
        public General<UserViewModel> GetUsers();
        public General<UserViewModel> Insert(UserViewModel newUser);
        public General<UserViewModel> Update(int id, UserViewModel user);
        public General<UserViewModel> Delete(int id);
    }
}
