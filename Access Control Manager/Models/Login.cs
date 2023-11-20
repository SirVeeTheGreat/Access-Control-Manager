using System.ComponentModel.DataAnnotations;


namespace Access_Control_Manager.Models
{
    public class Login
    {
        

        [Required, Display(Name = "Email Address")]
        public string Email { get; set; }

       
        [Required, Display(Name = "Password")]
        [DataType(DataType.Password)] 
        public string Password { get; set; }

        [Display(Name = "Stay logged-in")]
        public bool RememberMe { get; set; } = false;

       
    }
}
