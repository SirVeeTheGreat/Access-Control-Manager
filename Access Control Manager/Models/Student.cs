using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Access_Control_Manager.Models
{
    public class Student
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long StudentNumber { get; set; }

        public string Campus { get; set; }
       
        public bool CheckedOut { get; set; }
     
        public bool HasDevice { get; set; }
      
        public DateTime CheckIn { get; set; } 

        public DateTime CheckOut { get; set; }

        public DateTime DateRegistered { get; set; }

        public string QRCodePath { get; set; }

        public ICollection<Device> Devices { get; set; }

        public string UniqueGeneratedCode {get; set; }
    }
}
