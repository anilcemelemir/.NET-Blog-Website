﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer.Concrete
{
    public class Comment
    {
        [Key]
        public int CommentID { get; set; }
        public string CommentUserName { get; set; }
        public string CommentTitle { get; set; }
        public string CommmentContent { get; set; }
        public DateTime CommentDate { get; set; }
        public int BlogScore { get; set; }
        public bool CommentStatus { get; set; }
        public int BlogID { get; set; }
        public Blog Blog { get; set; }
    }
}
