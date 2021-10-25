using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using Newspaper.Admin.Controllers;
using Newspaper.Admin.Services;
using Newspaper.ViewModels.TopicViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Newspaper.Admin.Admin.Controllers
{
    public class TopicController:BaseController
    {
        private readonly ITopicApi _iTopicApi;
        private readonly IConfiguration _configuration;
        private readonly IPostApi _iPostApi;
        public TopicController(
            ITopicApi iTopicApi,
            IConfiguration configuration,
            IPostApi iPostApi,
            IAuthorApi iAuthorApi
            )
        {
            _iTopicApi = iTopicApi;
            _iPostApi = iPostApi;
            _configuration = configuration;
        }
        public async Task<IActionResult> Index(string keyword, int pageIndex = 1, int pageSize = 10)
        {
            if (!ModelState.IsValid)
                return View();

            var request = new GetTopicPagingRequest()
            {
                Keyword = keyword,
                PageIndex = pageIndex,
                PageSize = pageSize,
            };

            var data = await _iTopicApi.GetPostPaging(request);
            ViewBag.Keyword = keyword;

            if (TempData["result"] != null)
            {
                ViewBag.SuccessMsg = TempData["result"];
            }
            return View(data);

        }
        public async Task<IActionResult> Create()
        {
            var posts = await _iPostApi.GetAll();
            var topicCreateRequest = new TopicCreateRequest();
            ViewBag.address = new SelectList(posts, "Id", "Title");
            topicCreateRequest.Post = posts;
            return View(topicCreateRequest);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create( TopicCreateRequest request)
        {
            if (!ModelState.IsValid)
                return View();
            var posts = await _iPostApi.GetAll();

            request.Post = posts;
  
            var result = await _iTopicApi.Create(request);

            if (!result.IsSuccessed)
            {
                ModelState.AddModelError("", result.Message);
                return View(request);
            }
            if (result.IsSuccessed)
            {
                TempData["result"] = result.ResultObj;
                return RedirectToAction("Index", "Topic");
            }
            return View(request);
        }
        public async Task<IActionResult> Details(int id)
        {
            var result = await _iTopicApi.GetById(id);
            return View(result);
        }
        public async void SetViewBag(int? seletedId = null)
        {
            var postAdd = await _iPostApi.GetAll();

            ViewBag.HousldRepre = new SelectList(postAdd, "Id", "Name", seletedId);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var topic = await _iTopicApi.GetById(id);
            var request = new TopicEditRequest()
            {
                Title = topic.Title,
                Description = topic.Description,
                CreatedDate = topic.CreatedDate,
            };
            return View(request);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, TopicEditRequest request)
        {
            if (!ModelState.IsValid)
                return View(request);

            var result = await _iTopicApi.Update(id, request);
            if (result)
            {
                TempData["result"] = "Cập nhật thành công";
                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", "Cập nhật thất bại");
            return View(request);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var topic = await _iTopicApi.GetById(id);
            var request = new TopicDeleteRequest()
            {
                Id = topic.Id,
            };
            return View(request);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id, TopicDeleteRequest request)
        {
            if (!ModelState.IsValid)
                return View(request);

            var result = await _iTopicApi.Delete(request.Id);
            if (result)
            {
                TempData["result"] = "Cập nhật thành công";
                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", "Cập nhật thất bại");
            return View(request);
        }
    }
}
