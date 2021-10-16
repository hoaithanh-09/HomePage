using Newspaper.ViewModels.Common;
using Microsoft.EntityFrameworkCore;
using Newspaper.Data.EF;
using Newspaper.Data.Entities;
using Newspaper.Utilities;
using Newspaper.ViewModels.PostViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newspaper.ViewModels.AuthorViewModels;
using Newspaper.ViewModels.ImageViewModels;

namespace Newspaper.Services.Posts
{
    public class PostSV : IPostSV
    {
         private readonly NewspaperContext _context;
         public PostSV(NewspaperContext context)
         {
             _context = context;
         }
        
         public async Task<int> Create(PostCreateRequest request)
         {
             var postAdd = new Post()
             {
                 Id=request.Id,
                 Title=request.Title,
                 CreatedDate=request.CreatedDate,
                 ModifiedDate=request.ModifiedDate,
                 ImageId=request.ImageId,
                 AuthorId=request.AuthorId,
                 Content=request.Content,          
             };
             _context.Add(postAdd);
             await _context.SaveChangesAsync();
             return postAdd.Id;
         }

        public async Task<int> Delete(int id)
        {
            var post = await _context.Posts.FindAsync(id);
            if (post != null)
            {
                _context.Remove(post);

            }
            return await _context.SaveChangesAsync();
        }

        public async Task<List<PostVM>> GetAll()
         {
            var query = from f in _context.Posts select f;
            var post = await query.Select(x => new PostVM()
            {
                Id=x.Id,
                Title = x.Title,
                CreatedDate = x.CreatedDate,
                ModifiedDate = x.ModifiedDate,
                ImageId = x.ImageId,
                AuthorId = x.AuthorId,
                Content = x.Content,
            }).ToListAsync();

            return post;
        }

        public async Task<PostVM> GetById(int id)
         {
            var post = await _context.Posts.FindAsync(id);
            if (post == null)
                throw new MemberManagementException("Không tìm thấy!");
            var topicVM = new PostVM()
            {
                Title = post.Title,
                CreatedDate = post.CreatedDate,
                ModifiedDate = post.ModifiedDate,
                ImageId = post.ImageId,
                AuthorId = post.AuthorId,
                Content = post.Content,
            };
            return topicVM;
        }

        public async Task<PagedResult<PostVM>> GetPagedResult(GetPostPagingRequest request)
         {
            var query = from f in _context.Posts select f;

            if (!string.IsNullOrEmpty(request.Keyword))
                query = query.Where(x => x.Title.Contains(request.Keyword));

            int totalRow = await query.CountAsync();

            var data = await query.Skip((request.PageIndex - 1) * request.PageSize).Take(request.PageSize)
                .Select(x => new PostVM()
                {
                    Id = x.Id,
                    Title = x.Title,
                    CreatedDate = x.CreatedDate,
                    ModifiedDate = x.ModifiedDate,
                    ImageId = x.ImageId,
                    AuthorId = x.AuthorId,
                    Content = x.Content,
                }).ToListAsync();

            var pagedResult = new PagedResult<PostVM>()
            {
                PageIndex = request.PageIndex,
                PageSize = request.PageSize,
                TotalRecords = totalRow,
                Items = data
            };
            return pagedResult;
        }       
        public async Task<Post> Update(int id, PostEditRequest request)
        {
            var post = await _context.Posts.FindAsync(id);

            if (post == null)
            {
                throw new MemberManagementException("Không tìm thấy thông tin !");
            }

            post.Title = request.Title;
            post.CreatedDate = request.CreatedDate;
            post.ModifiedDate = request.ModifiedDate;
            post.ImageId = request.ImageId;
            post.AuthorId = request.AuthorId;
            post.Content = request.Content;    
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (GetById(id) == null)
                {
                    throw new MemberManagementException("Không tìm thấy thông tin");
                }
                else
                {
                    throw;
                }
            }
            return post;
        }
    }
}
