using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Access_Control_Manager.Models
{
    public class Campus
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Campus Name")]

        public string Name { get; set; }

        public ICollection<Lab> Labs { get; set; }

        public ICollection<User> Users { get; set; }    

    }
}
