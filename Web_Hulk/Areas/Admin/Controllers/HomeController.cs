using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Web_Hulk.Constants;

namespace Web_Hulk.Areas.Admin.Controllers
{
        [Area("Admin")]
        [Authorize(Roles = Roles.Admin)]
        public class HomeController : Controller
        {
            public IActionResult Index()
            {
                return View();
            }
            public IActionResult Alerts()
            {
                return View();
            }
            public IActionResult Accordion()
            {
                return View();
            }
            public IActionResult Badges()
            {
                return View();
            }
            public IActionResult Breadcrumbs()
            {
                return View();
            }
            public IActionResult Buttons()
            {
                return View();
            }
            public IActionResult Cards()
            {
                return View();
            }
            public IActionResult Carousel()
            {
                return View();
            }
            public IActionResult ListGroup()
            {
                return View();
            }
            public IActionResult Modal()
            {
                return View();
            }
            public IActionResult Tabs()
            {
                return View();
            }
            public IActionResult Pagination()
            {
                return View();
            }
            public IActionResult Progress()
            {
                return View();
            }
            public IActionResult Spinners()
            {
                return View();
            }
            public IActionResult Tooltips()
            {
                return View();
            }
        }
    }
