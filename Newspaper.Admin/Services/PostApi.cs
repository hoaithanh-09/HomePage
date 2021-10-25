using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newspaper.Utilities;
using Newspaper.ViewModels.Common;
using Newspaper.ViewModels.PostViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace HomePage.Services
{
    public class PostApi : BaseApi, IPostApi
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        public PostApi(IHttpClientFactory httpClientFactory,
                    IHttpContextAccessor httpContextAccessor,
                     IConfiguration configuration)
             : base(httpClientFactory, httpContextAccessor, configuration)
        {
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
            _httpClientFactory = httpClientFactory;
        }
        public async Task<ApiResult<string>> Create(PostCreateRequest request)
        {
            var sessions = _httpContextAccessor
                .HttpContext
                .Session
                .GetString(SystemConstants.AppSettings.Token);

            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration[SystemConstants.AppSettings.BaseAddress]);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);

            var requestContent = new MultipartFormDataContent();

            requestContent.Add(new StringContent(string.IsNullOrEmpty(request.Title) ? "" : request.Title.ToString()), "Title");
            requestContent.Add(new StringContent(string.IsNullOrEmpty(request.Content) ? "" : request.Content.ToString()), "Content");
            requestContent.Add(new StringContent(request.CreatedDate.ToString()), "CreateDate");
            requestContent.Add(new StringContent(request.ModifiedDate.ToString()), "ModifiedDate");

            requestContent.Add(new StringContent(request.Authors.ToString()), "AuthorId");
            requestContent.Add(new StringContent(request.ImageId.ToString()), "ImageId");


            //var json = JsonConvert.SerializeObject(request);
            //var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await client.PostAsync($"/api/Posts/CreatePost", requestContent);
            var result = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
                return new ApiSuccessResult<string>(result);
            return new ApiErrorResult<string>(result);
        }

        public async Task<bool> Delete(int id)
        {
            return await Delete($"api/Posts/DeletePost" + id);
        }

        public async  Task<PostVM> GetById(int id)
        {
            var data = await GetAsync<PostVM>(
             $"/api/Posts/GetByIdPost/{id}");

            return data;
        }

        public async Task<PagedResult<PostVM>> GetPostPaging(GetPostPagingRequest request)
        {
            var data = await GetAsync<PagedResult<PostVM>>(
             $"/api/Posts/pagingPost?pageIndex=" +
               $"{request.PageIndex}&pageSize={request.PageSize}&keyword={request.Keyword}");
            return data;
        }

        public async Task<bool> Update(int id, PostEditRequest request)
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["BaseAddress"]);
            var sessions = _httpContextAccessor.HttpContext.Session.GetString("JWT");

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);

            var json = JsonConvert.SerializeObject(request);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PutAsync($"/api/Posts/UpdatePosst/{id}", httpContent);
            var result = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
                return true;

            return false;
        }
    }
}
