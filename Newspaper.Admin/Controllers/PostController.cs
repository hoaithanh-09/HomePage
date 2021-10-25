using HomePage.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using Newspaper.ViewModels.PostViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HomePage.Controllers
{
    public class PostController : BaseController
    {
        private readonly IPostApi _iPostApi;
        private readonly IConfiguration _configuration;
        private readonly IImageApi _iImageApi;
        private readonly IAuthorApi _iAuthorApi;
        public PostController(
            IPostApi iPostApi,
            IConfiguration configuration,
            IImageApi iImageApi,
            IAuthorApi iAuthorApi
            )
        {
            _iPostApi = iPostApi;
            _configuration = configuration;
            _iImageApi = iImageApi;
            _iAuthorApi = iAuthorApi;
        }
        public async Task<IActionResult> Index(string keyword, int pageIndex = 1, int pageSize = 10)
        {
            if (!ModelState.IsValid)
                return View();

            var request = new GetPostPagingRequest()
            {
                Keyword = keyword,
                PageIndex = pageIndex,
                PageSize = pageSize,
            };

            var data = await _iPostApi.GetPostPaging(request);
            ViewBag.Keyword = keyword;

            if (TempData["result"] != null)
            {
                ViewBag.SuccessMsg = TempData["result"];
            }
            return View(data);

        }
        public async Task<IActionResult> Create()
        {
            var images = await _iImageApi.GetAll();
            var authors = await _iAuthorApi.GetAll();
            var postCreateRequest = new PostCreateRequest();
            ViewBag.address = new SelectList(images, "Id", "ImagePath");
            return View(postCreateRequest);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(PostCreateRequest request)
        {
            if (!ModelState.IsValid)
                return View();
            var images = await _iImageApi.GetAll();
            var authors = await _iAuthorApi.GetAll();

            request.Images = images;
            request.Authors = authors;
            var result = await _iPostApi.Create(request);

            if (!result.IsSuccessed)
            {
                ModelState.AddModelError("", result.Message);
                return View(request);
            }
            if (result.IsSuccessed)
            {
                TempData["result"] = result.ResultObj;
                return RedirectToAction("Index", "Post");
            }
            return View(request);
        }
        public async Task<IActionResult> Details(int id)
        {
            var result = await _iPostApi.GetById(id);
            return View(result);
        }
        public async void SetViewBag(int? seletedId = null)
        {
            var authorAdd = await _iAuthorApi.GetAll();

            ViewBag.HousldRepre = new SelectList(authorAdd, "Id", "Name", seletedId);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var post = await _iPostApi.GetById(id);
            var request = new PostEditRequest()
            {
                Title = post.Title,
                Content = post.Content,
                CreatedDate = post.CreatedDate,
                ModifiedDate = post.ModifiedDate,
            };
            return View(request);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, PostEditRequest request)
        {
            if (!ModelState.IsValid)
                return View(request);

            var result = await _iPostApi.Update(id, request);
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
            var post = await _iPostApi.GetById(id);
            var request = new PostDeleteRequest()
            {
                Id = post.Id,
            };
            return View(request);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id, PostDeleteRequest request)
        {
            if (!ModelState.IsValid)
                return View(request);

            var result = await _iPostApi.Delete(request.Id);
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
