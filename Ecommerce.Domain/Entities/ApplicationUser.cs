using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Ecommerce.Domain.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;

        public ApplicationUser(string firstName, string lastName, string email, string userName)
        {
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            UserName = userName;
        }

        private ApplicationUser()
        {
        }
    }
}
