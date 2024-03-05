using ABTestTracker.DataAccess.Models;

namespace ABTestTracker.DataAccess.Repository
{
    public interface IRepositoryDataAccess
    {
        Task<Guid> AddDeviceToDb(string deviceToken);
        Task<List<Price>> GetListOfPrices();
        Task AddPriceForExperiment(decimal share, decimal price);
        Task AddButtonColorForExperiment(decimal share, string valueColor);
        Task<List<ButtonColor>> GetListOfButtonColors();
        Task<int> GetAmountDevicesInBtnColorExp(string colorValue);
        Task<int> GetAmountDevicesInPriceExp(decimal priceValue);
        Task<bool> IsDeviceExistInPriceExperiment(string tokenDevice);
        Task<bool> IsDeviceExistInBtnColorExp(string tokenDevice);
        Task<string> FindButtonColorByDeviceToken(string deviceToken);
        Task<decimal> FindPriceByDeviceToken(string deviceToken);
        Task<string> AddDeviceToButtonColorsExp(Guid deviceId, Guid buttonColorId);
        Task<decimal> AddDeviceToPriceExp(Guid deviceId, Guid priceId);
        Task<Device?> FindDeviceByToken(string tokenDevice);
    }
}