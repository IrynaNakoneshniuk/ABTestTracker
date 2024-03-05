
using ABTestTracker.DataAccess.Models;
using ABTestTracker.DataAccess.Repository;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ABTestTracker.Services
{
    public class ButtonColorsExperiment : IButtonColorsExperiment
    {
        private readonly IRepositoryDataAccess _dataAccess;

        public ButtonColorsExperiment(IRepositoryDataAccess dataAccess)
        {
            _dataAccess = dataAccess;
        }

        //Determining the button color group is done by obtaining the count of already added devices and adding the 
        //current device participating in the experiment to the group with the least number of devices.

        public async Task<string> AddDeviceToExperiment(string deviceToken)
        {
            Guid minAmountColorId = Guid.Empty;
            var listOfButtonColors = await _dataAccess.GetListOfButtonColors();

            if (listOfButtonColors != null && listOfButtonColors.Any())
            {
                int min = await _dataAccess.GetAmountDevicesInBtnColorExp(listOfButtonColors[0].Value);
                minAmountColorId = listOfButtonColors[0].Id;

                foreach (var bc in listOfButtonColors)
                {
                    //get amount of divices for every color
                    var amountDevicesAtItem = await _dataAccess.GetAmountDevicesInBtnColorExp(bc.Value);
                    ///Determining min value
                    if (min > amountDevicesAtItem)
                    { 
                        min = amountDevicesAtItem;
                        minAmountColorId = bc.Id;
                    }
                }
            }

            Device? device = await _dataAccess.FindDeviceByToken(deviceToken);

            if (device == null)
            {
                Guid deviceId = await _dataAccess.AddDeviceToDb(deviceToken);
                device.Id = deviceId;
                device.DeviceToken = deviceToken;
            }
          
            string colorValue = await _dataAccess.AddDeviceToButtonColorsExp(device.Id, minAmountColorId);

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

        public async Task<List<ButtonColor>> GetListOfExperiment()
        {
            try
            {
                return await _dataAccess.GetListOfButtonColors();

            }catch (Exception ex)
            {
                Console.WriteLine("Error GetListOfButtonColors");
                 return new List<ButtonColor>();
            }
        }
    }
}
