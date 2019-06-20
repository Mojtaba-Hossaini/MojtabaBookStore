using System;

namespace MojtabaBookStore.Models.ViewModels.AccountViewModel
{
    public class UserSidebarViewModel
    {
        public string FullName { get; set; }
        public DateTime? RegisterDate { get; set; }
        public DateTime? LastVisit { get; set; }
        public string Image { get; set; }
    }
}
