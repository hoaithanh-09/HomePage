using Newspaper.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Newspaper.ViewModels.AuthorViewModels
{
    public class GetAuthorPagingRequest: PagingRequestBase
    {
        public string Keyword { get; set; }
    }
}
