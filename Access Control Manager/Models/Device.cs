using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Access_Control_Manager.Models
{
    public class Device
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

        [Column("StudentNumber")]
        public long StudentNumber { get; set; }
        [ForeignKey(nameof(StudentNumber))]
        public Student Student { get; set; }

    }
}
