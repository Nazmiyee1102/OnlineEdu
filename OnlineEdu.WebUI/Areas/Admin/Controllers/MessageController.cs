using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using OnlineEdu.WebUI.DTOs.MessageDtos;
using OnlineEdu.WebUI.Helpers;

namespace OnlineEdu.WebUI.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    [Area("Admin")]

    public class MessageController : Controller
    {
        private readonly HttpClient _client;

        public MessageController(IHttpClientFactory clientFactory)
        {
            _client = clientFactory.CreateClient("EduClient");
        }
        public async Task MessageDropdown()
        {
            var MessageList = await _client.GetFromJsonAsync<List<ResultMessageDto>>("Messages");

            List<SelectListItem> messages = (from x in MessageList
                                                     select new SelectListItem
                                                     {
                                                         Text = x.Name,
                                                         Value = x.MessageId.ToString(),
                                                     }).ToList();
            ViewBag.messages = messages;
        }

        public async Task<IActionResult> Index()
        {
            var values = await _client.GetFromJsonAsync<List<ResultMessageDto>>("Messages");
            return View(values);
        }

        public async Task<IActionResult> DeleteMessage(int id)
        {
            await _client.DeleteAsync("Messages/" + id);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> MessageDetail(int id)
        {
            var value = await _client.GetFromJsonAsync<ResultMessageDto>("Messages/" + id);
            return View(value);
        }
    }
}
