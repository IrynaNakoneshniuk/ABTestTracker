using Microsoft.AspNetCore.Mvc;

namespace ABTestTracker.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ExperimentController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
