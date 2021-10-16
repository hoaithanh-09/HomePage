using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Newspaper.ViewModels.TopicViewModels
{
    public class TopicEditRequest
    {
        public string Title { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string Description { get; set; }
    } 
}
