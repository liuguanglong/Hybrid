using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hybrid.Service.Shared.Data
{
    public class UserInfo
    {
        public string? Id { get; set; }
        public string? Email { get; set; }
        //public DateTime CreatedAt { get; set; }
        public String[]? Roles { get; set; }
        public String? Password { get; set; }

        public String getRoles()
        {
            return String.Join("|", Roles); 
        }
    }
}
