using System;
using System.Collections.Generic;

#nullable disable

namespace Newspaper.Data.Entities
{
    public partial class ImageInPost
    {
        public int ImageId { get; set; }
        public int PostId { get; set; }

        public virtual Image Image { get; set; }
        public virtual Post Post { get; set; }
    }
}
