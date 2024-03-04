using ABTestTracker.DataAccess.Data;
using ABTestTracker.DataAccess.Models;
using Microsoft.EntityFrameworkCore;

namespace ABTestTracker.DataAccess.Repository
{
    public class RepositoryDataAccess
    {
        private readonly ABTestContext _context;

        public RepositoryDataAccess(ABTestContext context)
        {
            this._context = context;
        }

        //public async Task<List<Device>> GetAllDevices()
        //{
        //    using(var context = _context)
        //    {
        //        return await context.Devices
        //            .FromSql("")
        //            .ToListAsync();
        //    }
        //}


    }
}
