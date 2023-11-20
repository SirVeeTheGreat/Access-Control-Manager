using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Access_Control_Manager.Models
{
    public class DeviceRecord
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Device Manufacture")]
        public string Manufacture { get; set; }

        [Display(Name = "Device Model")]
        public string Model { get; set; }

        [Required]
        [Display(Name = "Device Serial Number")]
        public string SerialNumber { get; set; }

        [Display(Name = "Device Type")]
        public string Type { get; set; }
        [Display(Name = "Any Accessories")]
        public string Accessories { get; set; }
        [Required]
        public DateTime Created { get; set; }

        [Column("StudentRecordId")]
        public int StudentRecordId { get; set; }

        public StudentRecord Student { get; set; }

    }
}
