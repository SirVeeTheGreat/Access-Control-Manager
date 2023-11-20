using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Access_Control_Manager.Models
{
    public class Activity
    {
        [Key]
        public int key { get; set; }

        public string ActivityDoneByStuff { get; set; } 

        public DateTime DateLogged { get;}  = DateTime.Now;

        [Column("StaffActivityId")]
        public int StaffActivityLogId { get; set; }

        [ForeignKey(nameof(StaffActivityLogId))]
        public StaffActivityLog StaffActivityLog { get; set; }

    }
}
