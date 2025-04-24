using BusinessLogic.DTO;
using BusinessLogic.IServices;
using Client3.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Client3.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IMessageService _messageService;

        public HomeController(IMessageService messageService,  ILogger<HomeController> logger)
        {
            _messageService = messageService;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            var vm = await _messageService.GetMessagesFromLast10minutesVM();
            return View(vm);

        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
