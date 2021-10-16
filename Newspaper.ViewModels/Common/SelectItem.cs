using System;
using System.Collections.Generic;
using System.Text;

namespace Newspaper.ViewModels.Common
{
   public class SelectItem
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public bool Selected { get; set; }

        public object Select()
        {
            throw new NotImplementedException();
        }
    }
}
