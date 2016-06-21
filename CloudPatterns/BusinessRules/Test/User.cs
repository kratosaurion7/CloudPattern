using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudPatterns.BusinessRules.Test
{
    public class User
    {
        public string Username { get; set; }

        public List<UserRole> Roles { get; set; }

        public User()
        {
            Roles = new List<UserRole>();
        }
    }
}
