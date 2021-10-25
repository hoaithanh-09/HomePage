using Newspaper.ViewModels.AuthorViewModels;
using Newspaper.ViewModels.ImageInPostViewModels;
using Newspaper.ViewModels.ImageViewModels;
using Newspaper.ViewModels.PostInTopicViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Newspaper.ViewModels.PostViewModels
{
    public class PostVM
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int? AuthorId { get; set; }
        public string Content { get; set; }

        public ImageInPostVM ImageInPosts { get; set; }
        public AuthorVM AuthorVMs { get; set; }

    }
}
