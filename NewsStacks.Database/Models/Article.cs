﻿using System;
using System.Collections.Generic;

#nullable disable

namespace NewsStacks.Database.Models
{
    public partial class Article
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Tags { get; set; }
        public bool IsDraft { get; set; }
        public bool WriteDone { get; set; }
        public bool ReviewerDone { get; set; }
        public string ReviewerComments { get; set; }
        public bool EditorDone { get; set; }
        public string EditorComments { get; set; }
        public bool PublishDone { get; set; }
        public bool Active { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdateDate { get; set; }
    }
}