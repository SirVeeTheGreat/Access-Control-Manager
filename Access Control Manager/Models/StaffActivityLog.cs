using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Access_Control_Manager.Models
{
    public class StaffActivityLog
    {
        [Key]
        public int Id { get; set; } 

        public string CurrentlyLogInStaffNumber { get; set; } 
             
        public DateTime LastLoginTime { get; } = DateTime.Now;

        public DateTime LastLogoutTime { get; set;}

        public bool isLoggedIn { get; set; }

        public ICollection<Activity> Activities { get; set; }

        [Column("UserId")]
        public int UserId { get; set; }

        [ForeignKey(nameof(UserId))]
        public User User { get; set; }  

    }
}
