using System;
using System.Collections.Generic;

#nullable disable

namespace NewsStacks.Database.Models
{
    public partial class ArticleUser
    {
        public int Id { get; set; }
        public int ArticleId { get; set; }
        public int UserRoleId { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }

        public virtual Article Article { get; set; }
        public virtual UserRole UserRole { get; set; }
    }
}
