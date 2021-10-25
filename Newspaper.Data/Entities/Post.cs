using System;
using System.Collections.Generic;

#nullable disable

namespace Newspaper.Data.Entities
{
    public partial class Post
    {
        public Post()
        {
            ImageInPosts = new HashSet<ImageInPost>();
            PostInTopics = new HashSet<PostInTopic>();
        }

        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int? AuthorId { get; set; }
        public string Content { get; set; }

        public virtual Author Author { get; set; }
        public virtual ICollection<ImageInPost> ImageInPosts { get; set; }
        public virtual ICollection<PostInTopic> PostInTopics { get; set; }
    }
}
