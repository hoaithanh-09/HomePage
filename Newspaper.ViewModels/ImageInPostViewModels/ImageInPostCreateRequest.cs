using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Newspaper.ViewModels.ImageInPostViewModels
{
    public class ImageInPostCreateRequest
    {
        public int ImageId { get; set; }
        public int PostId { get; set; }
    }
}
