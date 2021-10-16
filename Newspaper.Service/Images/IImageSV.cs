using Newspaper.Data.Entities;
using Newspaper.ViewModels.Common;
using Newspaper.ViewModels.ImageViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Newspaper.Services.Images
{
    public interface IImageSV
    {
        //Image
        Task<int> CreateImage(ImageCreateRequest request);
        Task<int> DeleteImage(int id);
        Task<Image> UpadateImage(int id, ImageEditRequest request);
        Task<ImageVM> GetImageById(int id);
        Task<List<ImageVM>> GetAllImages();
        Task<PagedResult<ImageVM>> GetPagedResultImage(GetImagePagingRequest request);
    }
}
