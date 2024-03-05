using ABTestTracker.DataAccess.Repository;
using Microsoft.AspNetCore.Mvc;

namespace ABTestTracker.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ExperimentController : Controller
    {
        private readonly IRepositoryDataAccess repositoryDataAccess;

        public ExperimentController(IRepositoryDataAccess repositoryDataAccess)
        {
            this.repositoryDataAccess = repositoryDataAccess;   
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
           int prices = await repositoryDataAccess.GetAmountDevicesInPriceExp(10);

            return Ok(prices);
        }

        [HttpPost]
        public async Task<IActionResult> AddingPrice(decimal share,decimal price)
        {
            await repositoryDataAccess.AddPriceForExperiment(price,share);

            return Ok();
        }

        [HttpPost ("/coloradd")]
        public async Task<IActionResult> AddingColor(decimal share, string valueColor)
        {
            await repositoryDataAccess.AddButtonColorForExperiment(share, valueColor);


            return Ok();
        }

        [HttpGet("/colorget")]
        public async Task<IActionResult> GetListColor()
        {
            List<DataAccess.Models.ButtonColor> prices = await repositoryDataAccess.GetListOfButtonColors();

            return Ok(prices);
        }
    }
}
