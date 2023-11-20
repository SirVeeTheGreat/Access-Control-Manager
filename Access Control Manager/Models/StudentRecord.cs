using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Access_Control_Manager.Models
{
    public class StudentRecord
    {
        public int Id { get; set; }

        [Display(Name = "Campus")]
        public string Campus { get; set; }
        [Display(Name = "Accessed Campus")]
        public DateTime AccessedCampus { get; set; }
        [Display(Name = "Time-In")]
        public DateTime TimeIn {get; set; }
        [Display(Name = "Time-Out")]
        public DateTime TimeOut { get; set; }

        public ICollection<DeviceRecord> DeviceRecords {get; set;}

        [Column("StudentNumber")]
        public long StudentNumber {get; set; }
        [ForeignKey(nameof(StudentNumber))]
        public Student Student {get; set;}

    }

}
