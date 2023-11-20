using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Access_Control_Manager.Models
  {
    [NotMapped]
    public class SignUp
    {
        

        [Required]
        [EmailAddress]
        [Display(Name = "Email Address")]
        public string Email { get; set; }

        [Display(Name = "Campus")]
        [Required]
        public int CampusId { get; set; }

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

        public string Department { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password), Compare(otherProperty: nameof(Password), ErrorMessage = "Confirm Passwords do not match")]
        public string ConfirmPassword { get; set; }
    


    }
}
