﻿using System;
using System.Collections.Generic;

#nullable disable

namespace NewsStacks.Database.Models
{
    public partial class Role
    {
        public Role()
        {
            UserRoles = new HashSet<UserRole>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public bool Active { get; set; }

        public virtual ICollection<UserRole> UserRoles { get; set; }
    }
}
