using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newspaper.Services.Authors;
using Newspaper.ViewModels.AuthorViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Newspaper.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorsController : ControllerBase
    {
        private readonly IAuthorSV _authorSV;
        //dd
        public AuthorsController(IAuthorSV authorSV)
        {
            _authorSV = authorSV;
        }
        //author
        [HttpPost("CreateAuthor")]
        public async Task<ActionResult> CreateAuthor([FromBody] AuthorCreateRequest request)
        {
            var author = await _authorSV.CreateAuthor(request);
            return Ok(author);
        }
        [HttpGet("GetAllAuthors")]
        public async Task<IActionResult> GetAllAuthors()
        {
            var author = await _authorSV.GetAllAuthors();
            return Ok(author);
        }
        [HttpDelete("DeleteAuthor/{id}")]
        public async Task<ActionResult> DeleteAuthor(int id)
        {
            var author = await _authorSV.DeleteAuthor(id);
            return Ok();
        }

        [HttpGet("GetByIdAuthor/{id}")]
        public async Task<IActionResult> GetAuthorById(int id)
        {
            var author = await _authorSV.GetAuthorById(id);
            return Ok(author);
        }

        [HttpPut("UpdateAuthor/{id}")]
        public async Task<IActionResult> UpadateAuthor([FromRoute] int id, [FromBody] AuthorEditRequest request)
        {
            try
            {
                var author = await _authorSV.UpadateAuthor(id, request);
                return Ok(author);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
        [HttpGet("GetPagedResultAuthor")]
        public async Task<IActionResult> GetPagedResultAuthor([FromQuery] GetAuthorPagingRequest request)
        {
            var author = await _authorSV.GetPagedResultAuthor(request);
            return Ok(author);
        }
    }
}
