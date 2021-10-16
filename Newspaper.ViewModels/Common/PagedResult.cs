using System;
using System.Collections.Generic;
using System.Text;

namespace Newspaper.ViewModels.Common
{
    public class PagedResult<T> : PagedResultBase
    {
        public List<T> Items { set; get; }
    }
}
