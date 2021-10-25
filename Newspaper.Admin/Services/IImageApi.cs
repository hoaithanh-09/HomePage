using Newspaper.ViewModels.Common;
using Newspaper.ViewModels.ImageViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HomePage.Services
{
    public interface IImageApi
    {
        Task<List<ImageVM>> GetAll();
        Task<PagedResult<ImageVM>> GetImagePaging(GetImagePagingRequest request);

        Task<bool> Create(ImageCreateRequest request);

        Task<bool> Update(int id, ImageEditRequest request);
        Task<ImageVM> GetById(int id);
        Task<bool> Delete(int id);
    }
}
