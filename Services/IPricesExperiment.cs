
namespace ABTestTracker.Services
{
    public interface IPricesExperiment
    {
        Task<decimal> AddDeviceToExperiment(string deviceToken);
        Task<decimal> GetPriceForExistDevice(string deviceToken);
        Task<bool> IsDeviceExistInCurrentExperiment(string deviceToken);
    }
}