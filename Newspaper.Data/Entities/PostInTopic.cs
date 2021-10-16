using System;
using System.Collections.Generic;

#nullable disable

namespace Newspaper.Data.Entities
{
    public partial class PostInTopic
    {
        public int TopicId { get; set; }
        public int PostId { get; set; }

        public virtual Post Post { get; set; }
        public virtual Topic Topic { get; set; }
    }
}
