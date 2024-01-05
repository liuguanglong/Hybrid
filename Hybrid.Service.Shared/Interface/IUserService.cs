using Hybrid.Service.Shared.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hybrid.Service.Shared.Interface
{
    public interface IUserService
    {
        Task<UserInfo[]?> GeUsersAsync();
        Task UpdateRolesAsync(UserInfo userInfo);
    }
}
