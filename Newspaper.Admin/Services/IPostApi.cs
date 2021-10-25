using Newspaper.ViewModels.Common;
using Newspaper.ViewModels.PostViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HomePage.Services
{
    public interface IPostApi
    {
        Task<PagedResult<PostVM>> GetPostPaging(GetPostPagingRequest request);

        Task<ApiResult<string>> Create(PostCreateRequest request);

        Task<bool> Update(int id, PostEditRequest request);
        Task<PostVM> GetById(int id);
        Task<bool> Delete(int id);
    }
}
