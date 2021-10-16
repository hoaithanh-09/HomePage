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

namespace Newspaper.Services.Topics
{
    public class TopicSV : ITopicSV
    {
        private readonly NewspaperContext _context;
        public TopicSV(NewspaperContext context)
        {
            _context = context;
        }

        public async Task<int> Create(TopicCreateRequest request)
        {
            var topicAdd = new Topic()
            {
                Id = request.Id,
                CreatedDate = request.CreatedDate,
                Title = request.Title,
                Description=request.Description,
            };
            _context.Add(topicAdd);
            await _context.SaveChangesAsync();
            return topicAdd.Id;
        }

        public async Task<int> Delete(int id)
        {
            var topic = await _context.Topics.FindAsync(id);
            if (topic != null)
            {
                _context.Remove(topic);

            }
            return await _context.SaveChangesAsync();
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
            var topic = await _context.Topics.FindAsync(id);
            if (topic == null)
                throw new MemberManagementException("Không tìm thấy!");
            var topicVM = new TopicVM()
            {
                CreatedDate = topic.CreatedDate,
                Title = topic.Title,
                Description = topic.Description
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
        public async Task<int> AddPost(PostInTopicCreateRequest request)
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

    }
}
