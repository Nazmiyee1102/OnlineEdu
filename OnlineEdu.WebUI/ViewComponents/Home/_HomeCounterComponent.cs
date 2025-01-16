using Microsoft.AspNetCore.Mvc;
using OnlineEdu.WebUI.Helpers;
using OnlineEdu.WebUI.Services.UserServices;

namespace OnlineEdu.WebUI.ViewComponents.Home
{
    public class _HomeCounterComponent: ViewComponent
    {
        private readonly HttpClient _client;
        private readonly IUserService _userService;


        public _HomeCounterComponent(IHttpClientFactory clientFactory, IUserService userService)
        {
            _client = clientFactory.CreateClient("EduClient");
            _userService = userService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            ViewBag.blogCount = await _client.GetFromJsonAsync<int>("blogs/GetBlogCount");
            
            ViewBag.courseCount = await _client.GetFromJsonAsync<int>("courses/GetCourseCount");
            
            ViewBag.courseCategoryCount = await _client.GetFromJsonAsync<int>("courseCategories/GetCourseCategoryCount");

            ViewBag.testimonialCount = await _client.GetFromJsonAsync<int>("testimonials/GetTestimonialCount");
            
            return View();
        }
    }
}
