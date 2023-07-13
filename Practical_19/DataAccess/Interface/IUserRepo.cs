using DataAccess.Enum;
using DataAccess.Models;
using DataAccess.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Interface
{
    public interface IUserRepo
    {
        Task<UserManagerResponse> RegisterUserAsync(Users model);
        Task<UserManagerResponse> LogoutUserAsync(LogoutViewModel model);
        Task<UserManagerResponse> LoginUserAsync(LoginViewModel model);
    }
}
