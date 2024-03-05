using ABTestTracker.DataAccess.Models;
using ABTestTracker.DataAccess.Repository;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System;

namespace ABTestTracker.Services
{
    public class PricesExperiment : IPricesExperiment
    {
        private readonly IRepositoryDataAccess _dataAccess;

        public PricesExperiment(IRepositoryDataAccess dataAccess)
        {
            _dataAccess = dataAccess;
        }

        public async Task<bool> IsDeviceExistInCurrentExperiment(string deviceToken)
        {
            return await _dataAccess.IsDeviceExistInPriceExperiment(deviceToken);
        }

        public async Task<decimal> GetPriceForExistDevice(string deviceToken)
        {
            try
            {
                decimal result = await _dataAccess.FindPriceByDeviceToken(deviceToken);

                if (result == null)
                {
                    throw new Exception($"Price not found for exist device, token={deviceToken}");
                }

                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in spFindPriceByToken: {ex.Message}");

                throw;
            }
        }
        //Determine the price group by using the Random class so that the share
        //of the price in the experiment corresponds to the probability of randomly selecting a range.
        //For example, a price group with a share of 75% will be selected each time when a random number
        //falls within the range from 0 to 75, and a price group with a share of 10% will be selected when
        //the random number is within the range from 76 to 85.

        public async Task<Guid> GetPriceGroupId()
        {
            //get list of prices
            var listOfExperimentPrices = await _dataAccess.GetListOfPrices();
            Guid priceId = Guid.Empty;

            //Determining upper bound of the range
            decimal sumShare = 0;

            //Determining lower bound of the range
            decimal previusShare = 0;
            Random random = new Random();

            //Determining group of price
            int groupOfPrice = random.Next(101);

            foreach (var price in listOfExperimentPrices)
            {
                sumShare += price.Share;
                if (sumShare > previusShare && groupOfPrice <= sumShare)
                {
                    priceId = price.Id;
                    break;
                }

                previusShare = price.Share;
            }

            return priceId;
        }

        public async Task<decimal> AddDeviceToExperiment(string deviceToken)
        {
            Guid priceId = await GetPriceGroupId();

            if (priceId.Equals(Guid.Empty))
            {
                throw new Exception("Not found group of prices!");
            }

            Device? device= await _dataAccess.FindDeviceByToken(deviceToken);

            if (device == null)
            {
                Guid deviceId = await _dataAccess.AddDeviceToDb(deviceToken);
                device.Id = deviceId;
                device.DeviceToken = deviceToken;
            }

            decimal resultPrice = await _dataAccess.AddDeviceToPriceExp(device.Id, priceId);

            return resultPrice;
        }

        public async Task<List<Price>> GetListOfPrices()
        {
            try
            { 
                return await _dataAccess.GetListOfPrices();

            }catch (Exception ex)
            {
                Console.WriteLine($"Error in GetListOfPrices: {ex.Message}");
                return new List<Price>();
            }
        }

    }
}
