using ABTestTracker.DataAccess.Data;
using ABTestTracker.DataAccess.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;


namespace ABTestTracker.DataAccess.Repository
{
    public class RepositoryDataAccess : IRepositoryDataAccess
    {
        private readonly ABTestContext _context;

        public RepositoryDataAccess(ABTestContext context)
        {
            this._context = context;
        }

        public async Task<Guid> AddDeviceToDb(string deviceToken)
        {
            try
            {
                var tokenParameter = new SqlParameter("@device_token", deviceToken);
                var idParameter = new SqlParameter
                {
                    ParameterName = "@device_id",
                    SqlDbType = SqlDbType.UniqueIdentifier,
                    Direction = ParameterDirection.Output
                };

                await _context.Database.ExecuteSqlRawAsync("EXEC spCreateDevice @device_token, @device_id OUTPUT",
                        tokenParameter, idParameter);

                return (Guid)idParameter.Value;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding device to database: {ex.Message}");
                throw; 
            }
        }

        public async Task<List<Price>> GetListOfPrices()
        {
            try
            {
                return await _context.Prices.FromSql($"EXECUTE spGetListOfPrices")
                    .AsNoTracking()
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error stored procedure spGetListOfPrices: {ex.Message}");
 
                return new List<Price>();
            }
        }

        public async Task<List<ButtonColor>> GetListOfButtonColors()
        {
            try
            {
                return await _context.ButtonColors.FromSql($"EXECUTE spGetListOfButtonColors")
                    .AsNoTracking()
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error stored procedure spGetListOfButtonColors: {ex.Message}");

                return new List<ButtonColor>();
            }
        }

        public async Task AddPriceForExperiment(decimal price, decimal share)
        {
            try
            {
                await _context.Database.ExecuteSqlAsync($"EXECUTE spCreatePrice {price},{share}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding price: {ex.Message}");
            }
          
        }

        public async Task AddButtonColorForExperiment(decimal share, string valueColor)
        {
            try
            {
                await _context.Database.ExecuteSqlAsync($"EXECUTE spCreateButtonColors {valueColor},{share}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding price: {ex.Message}");
            }
        }

        public async Task<int> GetAmountDevicesInBtnColorExp(string colorValue)
        {
            try
            {
                var valueParameter = new SqlParameter("@value", colorValue); 
                var amountParameter = new SqlParameter
                {
                    ParameterName = "@amount",
                    SqlDbType = SqlDbType.Int,
                    Direction = ParameterDirection.Output
                };

                    await _context.Database.ExecuteSqlRawAsync("EXEC spGetAmountByButtonColor @value, @amount OUTPUT", 
                    valueParameter, amountParameter);

                   int amountDevices = (int)amountParameter.Value;

                   return amountDevices;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error stored procedure spGetAmountByButtonColor: {ex.Message}");

                throw;
            }
        }

        public async Task<int> GetAmountDevicesInPriceExp(decimal priceValue)
        {
            try
            {
                var valueParameter = new SqlParameter("@value", priceValue);
                var amountParameter = new SqlParameter
                {
                    ParameterName = "@amount",
                    SqlDbType = SqlDbType.Int,
                    Direction = ParameterDirection.Output
                };

                 await _context.Database.ExecuteSqlRawAsync("EXEC spGetAmountByPrice @value, @amount OUTPUT",
                    valueParameter, amountParameter);

                int amountDevices = (int)amountParameter.Value;

                return amountDevices;

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error stored procedure spGetAmountByButtonColor: {ex.Message}");

                throw;
            }
        }

        public async Task<bool> IsDeviceExistInBtnColorExp(string deviceToken)
        {
            try
            {
                var deviceTokenParameter = new SqlParameter("@device_token", deviceToken);
                var existenceParameter = new SqlParameter
                {
                    ParameterName = "@Existence",
                    SqlDbType = SqlDbType.Int,
                    Direction = ParameterDirection.Output
                };

                await _context.Database.ExecuteSqlRawAsync("EXEC spDeviceExistExperimentButtonColors @device_token, @Existence OUTPUT",
                    deviceTokenParameter, existenceParameter);

                return (int)existenceParameter.Value==1;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error checking device existence: {ex.Message}");
                throw;
            }
        }

        public async Task<bool> IsDeviceExistInPriceExperiment(string deviceToken)
        {
            try
            {
                var deviceTokenParameter = new SqlParameter("@device_token", deviceToken);
                var existenceParameter = new SqlParameter
                {
                    ParameterName = "@Existence",
                    SqlDbType = SqlDbType.Int,
                    Direction = ParameterDirection.Output
                };

                int deviceExists = await _context.Database.ExecuteSqlRawAsync(
                $"EXEC spDeviceExistExperimentPrice @device_token, @Existence OUTPUT", deviceTokenParameter, existenceParameter);

                return (int)existenceParameter.Value == 1;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error stored procedure spDeviceExistExperimentPrice: {ex.Message}");
                throw;
            }
        }

        public async Task<string> FindButtonColorByDeviceToken(string deviceToken)
        {
            try
            {
                var deviceTokenParameter = new SqlParameter("@device_token", deviceToken);
                var buttonColorParameter = new SqlParameter
                {
                    ParameterName = "@button_color_value",
                    SqlDbType = SqlDbType.VarChar,
                    Size = 10,
                    Direction = ParameterDirection.Output
                };

                await _context.Database.ExecuteSqlRawAsync(
                    "EXEC spFindButtonColorByToken @device_token, @button_color_value OUTPUT",
                    deviceTokenParameter, buttonColorParameter
                );

                if (buttonColorParameter.Value != null && buttonColorParameter.Value != DBNull.Value)
                {
                    return buttonColorParameter.Value?.ToString();
                }
                else
                {
                    throw new Exception($"Button color not found - stored procedure spFindButtonColorByToken, token = {deviceToken}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error calling stored procedure spFindButtonColorByToken: {ex.Message}");
                throw;
            }
        }


        public async Task<decimal> FindPriceByDeviceToken(string deviceToken)
        {
            try
            {
                var deviceTokenParameter = new SqlParameter("@device_token", deviceToken);
                var priceParameter = new SqlParameter
                {
                    ParameterName = "@price",
                    SqlDbType = SqlDbType.Decimal,
                    Precision = 5,
                    Scale = 2,
                    Direction = ParameterDirection.Output
                };

                await _context.Database.ExecuteSqlRawAsync(
                    "EXEC spFindPriceByToken @device_token, @price OUTPUT",
                    deviceTokenParameter, priceParameter
                );

                if (priceParameter.Value != null && priceParameter.Value != DBNull.Value)
                {
                    return (decimal)priceParameter.Value;
                }
                else
                {
                    throw new Exception($"Price not found - stored procedure spFindPriceByToken, token = {deviceToken}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error calling stored procedure spFindPriceByToken: {ex.Message}");
                throw;
            }
        }


        public async Task<string> AddDeviceToButtonColorsExp(Guid deviceId,Guid buttonColorId)
        {
            try
            {
                var buttonColorValueParameter = new SqlParameter
                {
                    ParameterName = "@button_color_value",
                    SqlDbType = SqlDbType.VarChar,
                    Size = 10,
                    Direction = ParameterDirection.Output
                };

                var deviceIdParameter = new SqlParameter("@device_id", deviceId);
                var buttonColorIdParameter = new SqlParameter("@button_color_id", buttonColorId);

                await _context.Database.ExecuteSqlRawAsync(
                    "EXEC spCreateExperimentButtonColors @device_id, @button_color_id, @button_color_value OUTPUT",
                    deviceIdParameter, buttonColorIdParameter, buttonColorValueParameter);

                return buttonColorValueParameter.Value?.ToString();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error calling stored procedure spCreateExperimentButtonColors: {ex.Message}");
                
                throw;
            }
        }

        public async Task<decimal> AddDeviceToPriceExp(Guid deviceId, Guid priceId)
        {
            try
            {
                var priceParameter = new SqlParameter
                {
                    ParameterName = "@price",
                    SqlDbType = SqlDbType.Decimal,
                    Direction = ParameterDirection.Output
                };

                var deviceIdParameter = new SqlParameter("@device_id", deviceId);
                var priceIdParameter = new SqlParameter("@price_id ", priceId);

                await _context.Database.ExecuteSqlRawAsync(
                    "EXEC spCreateExperimentPrices @device_id,@price_id, @price OUTPUT",
                    deviceIdParameter, priceIdParameter, priceParameter);

                return (decimal)priceParameter.Value;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error calling stored procedure spCreateExperimentPrices: {ex.Message}");

                throw;
            }
        }


        public async Task<Device?> FindDeviceByToken(string tokenDevice)
        {
            try
            {
                var tokenParameter = new SqlParameter("@device_token", tokenDevice);

                var devices = await _context.Devices
                .FromSqlRaw("EXEC spFindDeviceByToken @device_token", tokenParameter)
                .ToListAsync();

                return devices.FirstOrDefault();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error calling stored procedure spFindDeviceByToken: {ex.Message}");
                throw;
            }
        }
    }
}
