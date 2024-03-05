using ABTestTracker.DataAccess.Models;
using ABTestTracker.DataAccess.Repository;

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

        public async Task<decimal> AddDeviceToExperiment(string deviceToken)
        {
            var listOfExperimentPrices = await _dataAccess.GetListOfPrices();
            Guid priceId = Guid.Empty;
            decimal sumShare = 0;
            decimal previusShare = 0;

            Random random = new Random();
            int groupOfPrice = random.Next(101);

            foreach (var price in listOfExperimentPrices)
            {
                sumShare += price.Share;
                if (sumShare > previusShare && sumShare <= groupOfPrice)
                {
                    priceId = price.Id;
                    break;
                }

                previusShare = price.Share;
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

    }
}
