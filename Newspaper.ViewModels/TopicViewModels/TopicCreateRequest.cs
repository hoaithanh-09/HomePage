using Newspaper.ViewModels.PostViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Newspaper.ViewModels.TopicViewModels
{
    public class TopicCreateRequest
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string Description { get; set; }
        public int PostId { get; set; }
        public List<PostVM> Post { get; set; }
    }
}
