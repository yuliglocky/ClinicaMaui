using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicalUtp.Models
{
     public class UserDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Dni { get; set; }
        public string Cellphone { get; set; }
        public int Gender { get; set; }
        public string Blood { get; set; }
        public string Email { get; set; }
        public string Allergies { get; set; }
        public string Role { get; set; }
        public string Password { get; set; }

        public string Address { get; set; }
        public DateTime Birthday { get; set; }
        public bool IsDonor { get; set; }
        public bool IsDeleted { get; set; }
    }
}
