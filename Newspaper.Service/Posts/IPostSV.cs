using Newspaper.ViewModels.Common;
using Newspaper.Data.Entities;
using Newspaper.ViewModels.PostViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Newspaper.Services.Posts
{
    public interface IPostSV
    {
        //Post
        Task<List<PostVM>> GetAll();
        Task<int> Create(PostCreateRequest request);
        Task<int> Delete(int id);
        Task<PostVM> GetById(int id);
        Task<Post> Update(int id, PostEditRequest request);
        Task<PagedResult<PostVM>> GetPagedResult(GetPostPagingRequest request);

    }
}
