﻿using Newspaper.ViewModels.Common;
using Newspaper.Data.Entities;
using Newspaper.ViewModels.TopicViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newspaper.ViewModels.PostInTopicViewModels;

namespace Newspaper.Services.Topics
{
    public interface ITopicSV
    {
        Task<List<TopicVM>> GetAll();
        Task<ApiResult<string>> Create(TopicCreateRequest request);
        Task<string> Delete(int id);
        Task<TopicVM> GetById(int id);
        Task<Topic> Update(int id, TopicEditRequest request);
        Task<PagedResult<TopicVM>> GetPagedResult(GetTopicPagingRequest request);
        Task<int> AddPost(int topicId,PostInTopicCreateRequest request);
    }
}