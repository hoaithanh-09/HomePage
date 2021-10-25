using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Newspaper.ViewModels.PostViewModels
{
    public class PostEditRequest
    {
        public string Title { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int? AuthorId { get; set; }
        public string Content { get; set; }

    }
}
