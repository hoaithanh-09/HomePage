using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newspaper.Services.Images;
using Newspaper.ViewModels.ImageViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Newspaper.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController: ControllerBase
    {
        private readonly IImageSV _imageSV;
        public ImagesController(IImageSV imageSV)
        {
            _imageSV = imageSV;
        }
        //image
        [HttpPost("CreateImage")]
        public async Task<ActionResult> CreateImage([FromBody] ImageCreateRequest request)
        {
            var image = await _imageSV.CreateImage(request);
            return Ok(image);
        }
        [HttpGet("GetAllImages")]
        public async Task<IActionResult> GetAllImages()
        {
            var image = await _imageSV.GetAllImages();
            return Ok(image);
        }
        [HttpDelete("DeleteImage/{id}")]
        public async Task<ActionResult> DeleteImage(int id)
        {
            var image = await _imageSV.DeleteImage(id);
            return Ok();
        }

        [HttpGet("GetByIdImage/{id}")]
        public async Task<IActionResult> GetImageById(int id)
        {
            var image = await _imageSV.GetImageById(id);
            return Ok(image);
        }

        [HttpPut("UpdateImage/{id}")]
        public async Task<IActionResult> UpadateImage([FromRoute] int id, [FromBody] ImageEditRequest request)
        {
            try
            {
                var image = await _imageSV.UpadateImage(id, request);
                return Ok(image);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
        [HttpGet("GetPagedResultImage")]
        public async Task<IActionResult> GetPagedResultImage([FromQuery] GetImagePagingRequest request)
        {
            var image = await _imageSV.GetPagedResultImage(request);
            return Ok(image);
        }
    }
}
