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
using Newspaper.ViewModels.ImageInPostViewModels;

namespace Newspaper.Services.Posts
{
    public class PostSV : IPostSV
    {
        private readonly NewspaperContext _context;
        public PostSV(NewspaperContext context)
        {
            _context = context;
        }

        public async Task<string> Delete(int id)
        {
            var post = await _context.Posts.FindAsync(id);
            if (post == null)
            {
                return "Không tìm thấy bài viết";
            }
            _context.Posts.Remove(post);
            await _context.SaveChangesAsync();
            return "Xóa thành công";
        }

        public async Task<List<PostVM>> GetAll()
        {
            var query = from f in _context.Posts select f;
            var post = await query.Select(x => new PostVM()
            {
                Id = x.Id,
                Title = x.Title,
                CreatedDate = x.CreatedDate,
                ModifiedDate = x.ModifiedDate,
                AuthorId = x.AuthorId,
                Content = x.Content,
            }).ToListAsync();

            return post;
        }

        public async Task<PostVM> GetById(int id)
        {
            var imageVM = new ImageVM();
            var authorVM = new AuthorVM();
            var post = await _context.Posts.Include(x => x.ImageInPosts).
                Where(x => x.Id == id).FirstOrDefaultAsync();
            if (post == null)
                throw new MemberManagementException("Không tìm thấy bài viết");

            var imageInPost = await _context.ImageInPosts.Where(x => x.PostId == id).FirstOrDefaultAsync();
            if (imageInPost != null)
            {
                var image = await _context.Images.Where(x => x.Id == imageInPost.ImageId).FirstOrDefaultAsync();
                imageVM = new ImageVM()
                {
                    Id=image.Id,
                    DateCreated = image.DateCreated,
                    FileSize = image.FileSize,
                    ImagePath = image.ImagePath,                   
                };
            }
           var author = await _context.Authors.FirstOrDefaultAsync(x => x.Id == post.AuthorId);
            authorVM = new AuthorVM()
            {
                Id = author.Id,
                Name=author.Name,
            };
        
            var imagePost = new ImageInPostVM()
            {
                Image = imageVM,
            };            

            var postVM = new PostVM()
            {
                Id = post.Id,
                CreatedDate = post.CreatedDate,
                Title = post.Title,               
                Content = post.Content,
                ModifiedDate = post.ModifiedDate,
                ImageInPosts = imagePost,
                AuthorVMs= authorVM,


            };
            return postVM;
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

        public async Task<int> AddImage(int postId,ImageInPostCreateRequest request)
        {
            var image = await _context.Images.FindAsync(request.ImageId);

            if (image == null)
            {
                throw new MemberManagementException("Thông tin không hợp lệ");
            }

            var imageInPost = new ImageInPost()
            {
                ImageId = image.Id,
                PostId = request.PostId,
            };

            _context.ImageInPosts.Add(imageInPost);
            await _context.SaveChangesAsync();
            return image.Id;
        }

        public async Task<int> AddAuthor(int postId, AuthorCreateRequest request)
        {
            var author = await _context.Authors.FindAsync(request.Id);

            if (author == null)
            {
                throw new MemberManagementException("Thông tin không hợp lệ");
            }

            _context.Authors.Add(author);
            await _context.SaveChangesAsync();
            return author.Id;
        }

        public async Task<ApiResult<string>>Create(PostCreateRequest request)
        {
            var post = await _context.Posts.FirstOrDefaultAsync(x => x.Title == request.Title);
            if (post != null)
            {
                return new ApiErrorResult<string>("Baì viết đã tồn tại");
            }

            post = new Post()
            {
                Id=request.Id,
                Title = request.Title,
                CreatedDate = request.CreatedDate,
                ModifiedDate = request.ModifiedDate,
                AuthorId = request.AuthorId,
                Content = request.Content,
            };

            if (request.ImageId != 0)
            {
                post.ImageInPosts = new List<ImageInPost>()
                { new ImageInPost()
                    {
                        ImageId = request.ImageId,
                        PostId = post.Id,
                    }
                };
            }      
            _context.Posts.Add(post);
            var a = await _context.SaveChangesAsync();
            if (a > 0)
            {
                return new ApiSuccessResult<string>("Tạo thành công");

            }
            return new ApiErrorResult<string>("Tạo mới thất bại");
        }
    }
}
