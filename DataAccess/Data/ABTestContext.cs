using ABTestTracker.DataAccess.Models;
using Microsoft.EntityFrameworkCore;


namespace ABTestTracker.DataAccess.Data
{
    public class ABTestContext:DbContext
    {
        public ABTestContext(DbContextOptions<ABTestContext> options) : base(options)
        {
            
        }

        public DbSet<ButtonColor> ButtonColors { get; set; }
        public DbSet<Device> Devices { get; set; }
        public DbSet<Price> Prices { get; set; }
        public DbSet<ExperimentButtonColor> ExperimentButtonColors { get; set; }
        public DbSet<ExperimentPrice> ExperimentPrices { get; set; }
    }
}
