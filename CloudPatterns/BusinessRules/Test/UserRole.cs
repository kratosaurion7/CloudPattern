using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudPatterns.BusinessRules.Test
{
    public class UserRole : IComparable<UserRole>
    {
        public string RoleName { get; set; }

        public int RolePrecedence { get; set; }

        public int CompareTo(UserRole other)
        {
            if(this.RolePrecedence > other.RolePrecedence)
            {
                return 1; // Current role is higher than other
            }
            else if (this.RolePrecedence == other.RolePrecedence)
            {
                return 0; // Current role is equal to other
            }
            else
            {
                return -1; // Current role is lower than other
            }
        }
    }
}
