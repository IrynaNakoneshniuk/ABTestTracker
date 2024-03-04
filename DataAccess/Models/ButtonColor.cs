using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ABTestTracker.DataAccess.Models
{
    [Table("button_colors")]
    [Index(nameof(Value), IsUnique = true)]
    public class ButtonColor
    {
        [Column("id")]
        [Key]
        public Guid Id { get; set; }

        [Column("value",TypeName = "varchar(10)")]
        public required string Value  { get; set; }

        [Column("share", TypeName ="decimal(5,2)")]
        public decimal Share {  get; set; }


        public List<ExperimentButtonColor> ButtonColorsExperiments { get; init; } = new();
    }
}
