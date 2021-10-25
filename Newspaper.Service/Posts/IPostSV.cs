using Newspaper.ViewModels.Common;
using Newspaper.Data.Entities;
using Newspaper.ViewModels.PostViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newspaper.ViewModels.ImageInPostViewModels;
using Newspaper.ViewModels.AuthorViewModels;

namespace Newspaper.Services.Posts
{
    public interface IPostSV
    {
        //Post
        Task<List<PostVM>> GetAll();
        Task<ApiResult<string>> Create(PostCreateRequest request);
        Task<string> Delete(int id);
        Task<PostVM> GetById(int id);
        Task<Post> Update(int id, PostEditRequest request);
        Task<PagedResult<PostVM>> GetPagedResult(GetPostPagingRequest request);
        Task<int> AddAuthor(int postId, AuthorCreateRequest request);
        Task<int> AddImage(int postId, ImageInPostCreateRequest request);


    }
}
