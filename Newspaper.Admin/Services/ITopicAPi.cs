using Newspaper.ViewModels.Common;
using Newspaper.ViewModels.TopicViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Newspaper.Admin.Services
{
    public interface ITopicApi
    {

        Task<PagedResult<TopicVM>> GetPostPaging(GetTopicPagingRequest request);

        Task<ApiResult<string>> Create(TopicCreateRequest request);

        Task<bool> Update(int id, TopicEditRequest request);
        Task<TopicVM> GetById(int id);
        Task<bool> Delete(int id);
    }
}
