
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Access_Control_Manager.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; } 

        [Required]
        [EmailAddress]
        [Display(Name = "Email Address")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Full Names")]
        public string Name { get; set; }
        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Surname")]
        public string Surname { get; set; }
      
        [Required]
        public string Role { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Student or Staff Number")]
        public string StudentStaffNumber { get; set; }

        
        [Column("CampusId")]
        public int CampusId {get; set; }

        public Campus Campus { get; set; }

        //AccountStatus Codes
        //1 - Active
        //0 - Disabled
        public int AccountStatus { get; set; } = 1;
       
        public string Department { get; set; }

        public ICollection<StaffActivityLog> ActivityLog { get; set; }

       
    }
}
