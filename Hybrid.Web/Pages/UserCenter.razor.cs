using Hybrid.Service.Shared.Data;
using Hybrid.Web.PlugIn;
using Hybrid.Web.Service;
using Microsoft.AspNetCore.Components;

namespace Hybrid.Web.Pages
{
    public partial class UserCenter
    {
        private IEnumerable<UserInfo> users;

        [Inject]
        public UserService service { get; set; }
        
        protected override async Task OnInitializedAsync()
        {
            users = await service.GeUsersAsync();
        }

        private async Task Edit(UserInfo user)
        {
        }
    }
}
