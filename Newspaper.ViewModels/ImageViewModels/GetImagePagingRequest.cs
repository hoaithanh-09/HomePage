using Newspaper.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Newspaper.ViewModels.ImageViewModels
{
    public class GetImagePagingRequest:PagingRequestBase
    {
        public string Keyword { get; set; }
    }
}
