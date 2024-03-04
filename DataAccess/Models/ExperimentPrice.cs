using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ABTestTracker.DataAccess.Models
{
    [Table("experiment_prices")]
    public class ExperimentPrice
    {
        [Column("id")]
        [Key]
        public Guid Id { get; set; }

        [ForeignKey(nameof(Device))]
        [Column("device_id")]
        public Guid DeviceId { get; set; }

        [ForeignKey(nameof(Price))]
        [Column("price_id")]
        public Guid PriceId { get; set; }


        public Device? Device { get; init; }
        public Price? Price { get; init; }
    }
}
