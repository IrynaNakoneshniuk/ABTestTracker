using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;

namespace ABTestTracker.DataAccess.Models
{
    [Table("devices")]
    [Index(nameof(DeviceToken),IsUnique = true)]
    public class Device
    {
       [Column("id")]
       [Key]
       public Guid Id { get; set; }

       [Column("device_token",TypeName = "varchar(100)")]
       public required string DeviceToken { get; set; }

       public ExperimentButtonColor? ButtonColor { get; init; }
       public ExperimentPrice? Price { get; init; }
    }
}
