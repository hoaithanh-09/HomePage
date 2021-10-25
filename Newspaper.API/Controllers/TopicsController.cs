using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newspaper.Services.Topics;
using Newspaper.ViewModels.PostInTopicViewModels;
using Newspaper.ViewModels.TopicViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Newspaper.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TopicsController: ControllerBase
    {
        private readonly ITopicSV _topicSV;
        //dd
        public TopicsController(ITopicSV topicSV)
        {
            _topicSV = topicSV;
        }
        [HttpGet("GetALl")]
        public async Task<IActionResult> GetAll()
        {
            var topic = await _topicSV.GetAll();
            return Ok(topic);
        }
        [HttpPost("Create")]
        public async Task<ActionResult> Create([FromBody] TopicCreateRequest request)
        {
            var topic = await _topicSV.Create(request);
            return Ok(topic);
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var topic = await _topicSV.Delete(id);
            return Ok();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var topic = await _topicSV.GetById(id);
            return Ok(topic);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] TopicEditRequest request)
        {
            try
            {
                var topic = await _topicSV.Update(id, request);
                return Ok(topic);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
        [HttpGet("paging")]
        public async Task<IActionResult> GetPaing([FromQuery] GetTopicPagingRequest request)
        {
            var topic = await _topicSV.GetPagedResult(request);
            return Ok(topic);
        }

        //add PostInTopic
        [HttpPost("Add PostInTopic")]
        public async Task<ActionResult> AddPost([FromRoute] int id,[FromBody] PostInTopicCreateRequest request)
        {
            var topic = await _topicSV.AddPost(id,request);
            return Ok(topic);
        }
    }
}
