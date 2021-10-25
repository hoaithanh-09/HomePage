using HomePage.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newspaper.ViewModels.AuthorViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HomePage.Controllers
{
    public class AuthorController : BaseController
    {
        private readonly IAuthorApi _authorApi;
        private readonly IConfiguration _configuration;


        public AuthorController(IAuthorApi authorApi,
            IConfiguration configuration)
        {
            _configuration = configuration;
            _authorApi = authorApi;
        }
        public async Task<IActionResult> Index(string keyword, int pageIndex = 1, int pageSize = 10)
        {
            var request = new GetAuthorPagingRequest()
            {
                Keyword = keyword,
                PageIndex = pageIndex,
                PageSize = pageSize,
            };
            var data = await _authorApi.GetAuthorPaging(request);
            ViewBag.Keyword = keyword;

            if (TempData["result"] != null)
            {
                ViewBag.SuccessMsg = TempData["result"];
            }
            return View(data);
        }

        // GET: FamiliesController/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var result = await _authorApi.GetById(id);
            return View(result);
        }

        // GET: FamiliesController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: FamiliesController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(AuthorCreateRequest request)
        {
            if (!ModelState.IsValid)
                return View();

            var result = await _authorApi.Create(request);
            if (!result)
            {
                ModelState.AddModelError("", "Tạo mới thất bại");
                return View(request);
            }
            if (result)
            {
                TempData["result"] = "Tạo mới thành công";
                return RedirectToAction("Index", "Author");
            }
            return View(request);
        }

        // GET: FamiliesController/Edit/5
        public async Task<ActionResult> Edit(int id)
        {

            var result = await _authorApi.GetById(id);

            if (result != null)
            {
                var author = new AuthorEditRequest()
                {
                    Name = result.Name,                    
                };
                return View(author);
            };
            return View(StatusCodes.Status400BadRequest);
        }

        // POST: FamiliesController/Edit/5
        [HttpPost]
        public async Task<ActionResult> Edit(int id, AuthorEditRequest request)
        {
            if (!ModelState.IsValid)
                return View(request);

            var result = await _authorApi.Update(id, request);
            if (result)
            {
                TempData["result"] = "Cập nhật thành công";
                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", "Cập nhật thất bại");
            return View(request);
        }

        // GET: FamiliesController/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
            var result = await _authorApi.GetById(id);

            if (result != null)
            {
                var author = new AuthorDeleteRequest()
                {
                    Id = id,
                };
                return View(author);
            }
            return View(StatusCodes.Status400BadRequest);
        }

        // POST: FamiliesController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int id, AuthorDeleteRequest request)
        {
            if (!ModelState.IsValid)
                return View();

            var result = await _authorApi.Delete(request.Id);
            if (result)
            {
                TempData["result"] = "Xóa tác giả thành công";
                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", "Xóa thất bại");
            return View();
        }
    }
}
