using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudPatterns.BusinessRules.Test
{
    public class TestCase
    {
        public void RunTests()
        {
            BusinessRulesRepository<User> UserRules = new BusinessRulesRepository<User>();
            
            UserRole adminRole = new UserRole() { RoleName = "Admin", RolePrecedence = 1 };
            UserRole readerRole = new UserRole() { RoleName = "Reader", RolePrecedence = 2 };

            User testUser = new User();
            testUser.Username = "tdube";
            testUser.Roles.Add(readerRole);

            BusinessRule<User> UserIsTristan = new GenericRule<User>("User is named tdube", u => u.Username == "tdube");
            UserIsTristan.ContinueProcessingOtherRules = false;
            UserRules.RuleSet.Add(UserIsTristan);

            BusinessRule<User> UserHasEnoughPermissions = new GenericRule<User>("User must be admin", u => u.Roles.Any(r => r.CompareTo(adminRole) <= 0));
            UserRules.RuleSet.Add(UserHasEnoughPermissions);

            var failed = UserRules.GetFailedRules(testUser);


            var result = UserRules.Apply(testUser);

            int i = 0;
        }
    }
}
