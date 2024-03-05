
using ABTestTracker.DataAccess.Repository;

namespace ABTestTracker.Services
{
    public class ButtonColorsExperiment : IButtonColorsExperiment
    {
        private readonly IRepositoryDataAccess _dataAccess;

        public ButtonColorsExperiment(IRepositoryDataAccess dataAccess)
        {
            _dataAccess = dataAccess;
        }

        public async Task<string> AddDeviceToExperiment(string deviceToken)
        {
            Guid minAmountColorId = Guid.Empty;
            var listOfButtonColors = await _dataAccess.GetListOfButtonColors();

            if (listOfButtonColors != null)
            {
                int min = await _dataAccess.GetAmountDevicesInBtnColorExp(listOfButtonColors[0].Value);
                minAmountColorId = listOfButtonColors[0].Id;

                listOfButtonColors.ForEach(async (bc) =>
                {
                    var amountDevicesAtItem = await _dataAccess.GetAmountDevicesInBtnColorExp(bc.Value);
                    if (min > amountDevicesAtItem)
                    {
                        min = amountDevicesAtItem;
                        minAmountColorId = bc.Id;
                    }
                }
              );
            }

            Guid deviceId = await _dataAccess.AddDeviceToDb(deviceToken);
            string colorValue = await _dataAccess.AddDeviceToButtonColorsExp(deviceId, minAmountColorId);

            return colorValue;
        }

        public async Task<bool> IsDeviceExistInCurrentExperiment(string deviceToken)
        {
            return await _dataAccess.IsDeviceExistInBtnColorExp(deviceToken);
        }

        public async Task<string> GetColorButtonForExistDevice(string deviceToken)
        {
            try
            {
                string result = await _dataAccess.FindButtonColorByDeviceToken(deviceToken);

                if (result == null)
                {
                    throw new Exception($"Color button not found for exist device, token={deviceToken}");
                }

                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in spFindButtonColorByToken: {ex.Message}");

                throw;
            }
        }
    }
}
