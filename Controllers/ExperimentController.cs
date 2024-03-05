using ABTestTracker.DataAccess.Models;
using ABTestTracker.DataAccess.Repository;
using ABTestTracker.Services;
using Microsoft.AspNetCore.Mvc;

namespace ABTestTracker.Controllers
{
    [Route("experiment")]
    [ApiController]
    public class ExperimentController : Controller
    {
        private readonly IButtonColorsExperiment _buttonColorsExperiment;
        private readonly IPricesExperiment _pricesExperiment;

        public ExperimentController(IButtonColorsExperiment buttonColorsExperiment, IPricesExperiment pricesExperiment)
        {
            this._buttonColorsExperiment = buttonColorsExperiment;   
            this._pricesExperiment = pricesExperiment;
        }

        [HttpGet("/button-color")]
        public async Task<IActionResult> ButtonColorsExperiment(string device_token) 
        {
            string buttonColor=string.Empty;

           if (await _buttonColorsExperiment.IsDeviceExistInCurrentExperiment(device_token))
            {
                buttonColor=await _buttonColorsExperiment.GetColorButtonForExistDevice(device_token);
            }
            else
            {
                buttonColor = await _buttonColorsExperiment.AddDeviceToExperiment(device_token);
            }
          
            return Ok(new { key = "button_color", value = buttonColor });
        }

        [HttpGet("/price")]
        public async Task<IActionResult> PricesExperiment(string device_token)
        {
            decimal price;

            if (await _pricesExperiment.IsDeviceExistInCurrentExperiment(device_token))
            {
                price = await _pricesExperiment.GetPriceForExistDevice(device_token);
            }
            else
            {
                price = await _pricesExperiment.AddDeviceToExperiment(device_token);
            }

            return Ok(new { key = "price", value = price });
        }

        [HttpGet("experiments")]
        public async Task<IActionResult> GetExperiments()
        {
            var listOfButtonColorExperiment = await _buttonColorsExperiment.GetListOfExperiment();
            var listOfPricesExperiment = await _pricesExperiment.GetListOfPrices();

            return Ok(new { prices = listOfPricesExperiment, buttonColors = listOfButtonColorExperiment });
        }
    }
}
