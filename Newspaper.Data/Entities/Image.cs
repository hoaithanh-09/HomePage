using System;
using System.Collections.Generic;

#nullable disable

namespace Newspaper.Data.Entities
{
    public partial class Image
    {
        public Image()
        {
            Posts = new HashSet<Post>();
        }

        public int Id { get; set; }
        public string ImagePath { get; set; }
        public DateTime? DateCreated { get; set; }
        public long? FileSize { get; set; }

        public virtual ICollection<Post> Posts { get; set; }
    }
}
