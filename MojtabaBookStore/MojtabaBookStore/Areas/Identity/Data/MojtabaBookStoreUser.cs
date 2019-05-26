using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace MojtabaBookStore.Areas.Identity.Data
{
    // Add profile data for application users by adding properties to the MojtabaBookStoreUser class
    public class MojtabaBookStoreUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDate { get; set; }
    }
}
