using System;
using System.Collections.Generic;

#nullable disable

namespace NewsStacks.Database.Models
{
    public partial class UserRole
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int RoleId { get; set; }
        public bool Active { get; set; }

        public virtual Role User { get; set; }
        public virtual User UserNavigation { get; set; }
    }
}
