using System;
using System.Collections.Generic;

#nullable disable

namespace Newspaper.Data.Entities
{
    public partial class Image
    {
        public Image()
        {
            ImageInPosts = new HashSet<ImageInPost>();
        }

        public int Id { get; set; }
        public string ImagePath { get; set; }
        public DateTime? DateCreated { get; set; }
        public long? FileSize { get; set; }

        public virtual ICollection<ImageInPost> ImageInPosts { get; set; }
    }
}
