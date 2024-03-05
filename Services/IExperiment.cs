namespace ABTestTracker.Services
{
    public interface IExperiment
    {
        Task AddDeviceToExperiment(string deviceToken);
        Task<bool> IsDeviceExistInCurrentExperiment(string deviceToken);
    }
}
