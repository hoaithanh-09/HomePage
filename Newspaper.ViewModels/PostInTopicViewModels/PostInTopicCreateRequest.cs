﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Newspaper.ViewModels.PostInTopicViewModels
{
    public class PostInTopicCreateRequest
    {
        public int TopicId { get; set; }
        public int PostId { get; set; }
    }
}
