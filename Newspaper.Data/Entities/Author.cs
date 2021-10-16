using System;
using System.Collections.Generic;

#nullable disable

namespace Newspaper.Data.Entities
{
    public partial class Author
    {
        public Author()
        {
            Posts = new HashSet<Post>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Post> Posts { get; set; }
    }
}
