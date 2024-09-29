using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineEdu.WebUI.DTOs.AboutDtos;
using OnlineEdu.WebUI.DTOs.BlogCategoryDtos;
using OnlineEdu.WebUI.Helpers;
using OnlineEdu.WebUI.Validators;

namespace OnlineEdu.WebUI.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    [Area("Admin")]

    public class BlogCategoryController : Controller
    {
        private readonly HttpClient _client = HttpClientInstance.CreateClient();

        public async Task<IActionResult> Index()
        {
            var values = await _client.GetFromJsonAsync<List<ResultBlogCategoryDto>>("blogCategories");
            return View(values);
        }

        public async Task<IActionResult> DeleteBlogCategory(int id)
        {
            await _client.DeleteAsync($"blogCategories/{id}");
            return RedirectToAction(nameof(Index));
        }

        public IActionResult CreateBlogCategory()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateBlogCategory(CreateBlogCategoryDto createBlogCategoryDto)
        {
            var validator = new BlogCategoryValidator();
            var result = await validator.ValidateAsync(createBlogCategoryDto);
            if (!result.IsValid)
            {
                ModelState.Clear();
                foreach (var x in result.Errors)
                {
                    ModelState.AddModelError(x.PropertyName, x.ErrorMessage);
                }
                return View();
            }

            await _client.PostAsJsonAsync("blogCategories", createBlogCategoryDto);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> UpdateBlogCategory(int id)
        {
            var values = await _client.GetFromJsonAsync<UpdateBlogCategoryDto>($"blogCategories/{id}");
            return View(values);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateBlogCategory(UpdateBlogCategoryDto updateBlogCategoryDto)
        {
            await _client.PutAsJsonAsync("blogCategories", updateBlogCategoryDto);
            return RedirectToAction(nameof(Index));
        }
    }
}
