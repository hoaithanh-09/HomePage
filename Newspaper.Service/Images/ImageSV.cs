using Microsoft.EntityFrameworkCore;
using Newspaper.Data.EF;
using Newspaper.Data.Entities;
using Newspaper.Utilities;
using Newspaper.ViewModels.Common;
using Newspaper.ViewModels.ImageViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Newspaper.Services.Images
{
    public class ImageSV : IImageSV
    {
        private readonly NewspaperContext _context;
        public ImageSV(NewspaperContext context)
        {
            _context = context;
        }
        public async Task<int> CreateImage(ImageCreateRequest request)
        {
            var image = new Image()
            {
                Id = request.Id,
                ImagePath = request.ImagePath,
                DateCreated = request.DateCreated,
                FileSize = request.FileSize
            };
            _context.Add(image);
            await _context.SaveChangesAsync();
            return image.Id;
        }
        public async Task<int> DeleteImage(int id)
        {
            var image = await _context.Images.FindAsync(id);
            if (image != null)
            {
                _context.Remove(image);

            }
            return await _context.SaveChangesAsync();
        }
        public async Task<List<ImageVM>> GetAllImages()
        {
            var query = from f in _context.Images select f;
            var image = await query.Select(x => new ImageVM()
            {
                Id = x.Id,
                DateCreated = x.DateCreated,
                FileSize = x.FileSize,
                ImagePath = x.ImagePath,
            }).ToListAsync();

            return image;
        }
        public async Task<ImageVM> GetImageById(int Id)
        {
            var image = await _context.Images.FindAsync(Id);
            if (image == null)
                throw new MemberManagementException("Không tìm thấy!");
            var imageVM = new ImageVM()
            {
                Id=image.Id,
                ImagePath = image.ImagePath,
                DateCreated = image.DateCreated,
                FileSize = image.FileSize,
            };
            return imageVM;
        }
        public async Task<PagedResult<ImageVM>> GetPagedResultImage(GetImagePagingRequest request)
        {
            var query = from f in _context.Images select f;

            if (!string.IsNullOrEmpty(request.Keyword))
                query = query.Where(x => x.ImagePath.Contains(request.Keyword));

            int totalRow = await query.CountAsync();

            var data = await query.Skip((request.PageIndex - 1) * request.PageSize).Take(request.PageSize)
                .Select(x => new ImageVM()
                {
                    Id = x.Id,
                    ImagePath = x.ImagePath,
                    FileSize = x.FileSize,
                    DateCreated = x.DateCreated,

                }).ToListAsync();

            var pagedResult = new PagedResult<ImageVM>()
            {
                PageIndex = request.PageIndex,
                PageSize = request.PageSize,
                TotalRecords = totalRow,
                Items = data
            };
            return pagedResult;
        }



        public async Task<Image> UpadateImage(int id, ImageEditRequest request)
        {
            var image = await _context.Images.FindAsync(id);

            if (image == null)
            {
                throw new MemberManagementException("Không tìm thấy thông tin !");
            }

            image.DateCreated = request.DateCreated;
            image.FileSize = request.FileSize;
            image.ImagePath = request.ImagePath;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (GetImageById(id) == null)
                {
                    throw new MemberManagementException("Không tìm thấy thông tin");
                }
                else
                {
                    throw;
                }
            }
            return image;
        }

    }
}