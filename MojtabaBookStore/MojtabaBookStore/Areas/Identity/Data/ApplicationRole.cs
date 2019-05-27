using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MojtabaBookStore.Areas.Identity.Data
{
    public class ApplicationRole : IdentityRole
    {
        public ApplicationRole()
        {

        }
        public ApplicationRole(string name) :base(name)
        {

        }
        public ApplicationRole(string name, string description):base(name)
        {
            Description = description;
        }
        public string Description { get; set; }

        public virtual List<ApplicationUserRole> Users { get; set; }
    }
}
