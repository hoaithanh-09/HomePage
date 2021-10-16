using System;
using System.Collections.Generic;

#nullable disable

namespace Newspaper.Data.Entities
{
    public partial class Post
    {
        public Post()
        {
            PostInTopics = new HashSet<PostInTopic>();
        }

        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int? ImageId { get; set; }
        public int? AuthorId { get; set; }
        public string Content { get; set; }

        public virtual Author Author { get; set; }
        public virtual Image Image { get; set; }
        public virtual ICollection<PostInTopic> PostInTopics { get; set; }
    }
}
