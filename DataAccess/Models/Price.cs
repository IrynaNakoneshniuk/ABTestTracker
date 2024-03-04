using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ABTestTracker.DataAccess.Models
{
    [Table("prices")]
    [Index(nameof(Value), IsUnique = true)]
    public class Price
    {
        [Column("id")]
        [Key]
        public Guid Id { get; set; }

        [Column("value",TypeName = "decimal(5,2)")]
        public required decimal Value { get; set; }

        [Column("share", TypeName = "decimal(5,2)")]
        public  decimal Share { get; set; }

        public List<ExperimentPrice> ExperimentsPrice { get; } = new();
    }
}
