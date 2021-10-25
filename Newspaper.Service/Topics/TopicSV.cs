using Newspaper.ViewModels.Common;
using Microsoft.EntityFrameworkCore;
using Newspaper.Data.EF;
using Newspaper.Data.Entities;
using Newspaper.Utilities;
using Newspaper.ViewModels.TopicViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newspaper.ViewModels.PostInTopicViewModels;
using Newspaper.ViewModels.PostViewModels;

namespace Newspaper.Services.Topics
{
    public class TopicSV : ITopicSV
    {
        private readonly NewspaperContext _context;
        public TopicSV(NewspaperContext context)
        {
            _context = context;
        }
        public async Task<string> Delete(int id)
        {
            var topic = await _context.Topics.FindAsync(id);
            if (topic == null)
            {
                return "Không tìm thấy chủ đề";
            }
            _context.Topics.Remove(topic);
            await _context.SaveChangesAsync();
            return "Xóa thành công";
        }

        public async Task<List<TopicVM>> GetAll()
        {
            var query = from f in _context.Topics select f;
            var topic = await query.Select(x => new TopicVM()
            {
                CreatedDate = x.CreatedDate,
                Title = x.Title,
                Description = x.Description
            }).ToListAsync();

            return topic;
        }

        public async Task<TopicVM> GetById(int id)
        {
            var postVM = new PostVM();
            var topic = await _context.Topics.Include(x => x.PostInTopics).
                Where(x => x.Id == id).FirstOrDefaultAsync();
            if (topic == null)
                throw new MemberManagementException("Không tìm thấy chủ đề");

            var postInTopic = await _context.PostInTopics.Where(x => x.TopicId == id).FirstOrDefaultAsync();
            if (postInTopic != null)
            {
                var post = await _context.Posts.Where(x => x.Id == postInTopic.PostId).FirstOrDefaultAsync();
                postVM = new PostVM()
                {
                    Id = post.Id,
                    CreatedDate = post.CreatedDate,
                    Content = post.Content,
                    AuthorId = post.AuthorId,
                    ModifiedDate=post.ModifiedDate,
                    Title=post.Title,
                };
            }

            var postTopic = new PostInTopicVM()
            {
                Post = postVM,
            };

            var topicVM = new TopicVM()
            {
                Id = topic.Id,
                CreatedDate = topic.CreatedDate,
                Description = topic.Description,
                PostInTopics = postTopic,
                Title = topic.Title,
            };
            return topicVM;
        }

        public async Task<PagedResult<TopicVM>> GetPagedResult(GetTopicPagingRequest request)
        {
            var query = from f in _context.Topics select f;

            if (!string.IsNullOrEmpty(request.Keyword))
                query = query.Where(x => x.Title.Contains(request.Keyword));

            int totalRow = await query.CountAsync();

            var data = await query.Skip((request.PageIndex - 1) * request.PageSize).Take(request.PageSize)
                .Select(x => new TopicVM()
                {
                    Id = x.Id,
                    CreatedDate = x.CreatedDate,
                    Title = x.Title,
                    Description = x.Description,
                }).ToListAsync();

            var pagedResult = new PagedResult<TopicVM>()
            {
                PageIndex = request.PageIndex,
                PageSize = request.PageSize,
                TotalRecords = totalRow,
                Items = data
            };
            return pagedResult;
        }

        public async Task<Topic> Update(int id, TopicEditRequest request)
        {
            var topic = await _context.Topics.FindAsync(id);

            if (topic == null)
            {
                throw new MemberManagementException("Không tìm thấy thông tin !");
            }

            topic.CreatedDate = request.CreatedDate;
            topic.Title = request.Title;
            topic.Description = request.Description;
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
            return topic;
        }

        public async Task<int> AddPost(int topicId, PostInTopicCreateRequest request)
        {
            var post = await _context.Posts.FindAsync(request.PostId);

            if (post == null)
            {
                throw new MemberManagementException("Thông tin không hợp lệ");
            }

            var postInTopic = new PostInTopic()
            {
                PostId = post.Id,
                TopicId = request.TopicId,
            };

            _context.PostInTopics.Add(postInTopic);
            await _context.SaveChangesAsync();
            return post.Id;
        }

        public async Task<ApiResult<string>> Create(TopicCreateRequest request)
        {
            var topic = await _context.Topics.FirstOrDefaultAsync(x => x.Title == request.Title);
            if (topic != null)
            {
                return new ApiErrorResult<string>("Chủ đề đã tồn tại");
            }

            topic = new Topic()
            {
                Title = request.Title,
                CreatedDate = request.CreatedDate,
                Description = request.Description,
            };

            if (request.PostId != 0)
            {
                topic.PostInTopics = new List<PostInTopic>()
                { new PostInTopic()
                    {
                        PostId = request.PostId,
                        TopicId = topic.Id,
                    }
                };
            }
            _context.Topics.Add(topic);
            var a = await _context.SaveChangesAsync();
            if (a > 0)
            {
                return new ApiSuccessResult<string>("Tạo thành công");

            }
            return new ApiErrorResult<string>("Tạo mới thất bại");
        }
    }
}
