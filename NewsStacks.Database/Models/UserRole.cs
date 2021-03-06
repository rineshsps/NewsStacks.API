using System;
using System.Collections.Generic;

#nullable disable

namespace NewsStacks.Database.Models
{
    public partial class UserRole
    {
        public UserRole()
        {
            ArticleUsers = new HashSet<ArticleUser>();
        }

        public int Id { get; set; }
        public int UserId { get; set; }
        public int RoleId { get; set; }
        public bool Active { get; set; }

        public virtual Role Role { get; set; }
        public virtual User User { get; set; }
        public virtual ICollection<ArticleUser> ArticleUsers { get; set; }
    }
}
