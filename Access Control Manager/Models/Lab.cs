using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Access_Control_Manager.Models
{
    public class Lab
    {
        [Key]
        public int Id { get; set; }

             
        [Required]
        [Display(Name = "Building Name")]
        public string Building { get; set; }

        [Required]
        [Display(Name = "Room Name/Number")]
        public string RoomID { get; set; }

        [Column("CampusId")]
        public int CampusId { get; set; }   

        [ForeignKey(nameof(CampusId))]
        public Campus Campus{ get; set; }

    }
}
