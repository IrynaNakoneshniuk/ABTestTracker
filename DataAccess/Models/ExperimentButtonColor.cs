using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ABTestTracker.DataAccess.Models
{
    [Table("experiment_button_colors")]
    public class ExperimentButtonColor
    {
        [Column("id")]
        [Key]
        public Guid Id { get; set; }

        [ForeignKey(nameof(Device))]
        [Column("device_id")]
        public Guid DeviceId { get; set; }

        [ForeignKey(nameof(ButtonColor))]
        [Column("button_color_id")]
        public Guid ButtonColorId { get; set; }


        public Device? Device { get; init; }
        public ButtonColor? ButtonColor { get; init; }
    }
}
