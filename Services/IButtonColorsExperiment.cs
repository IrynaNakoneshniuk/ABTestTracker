
using ABTestTracker.DataAccess.Models;

namespace ABTestTracker.Services
{
    public interface IButtonColorsExperiment
    {
        Task<string> AddDeviceToExperiment(string deviceToken);
        Task<string> GetColorButtonForExistDevice(string deviceToken);
        Task<bool> IsDeviceExistInCurrentExperiment(string deviceToken);
        Task<List<ButtonColor>> GetListOfExperiment();
    }
}