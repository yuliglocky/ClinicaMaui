using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicalUtp.Models
{
    public class ReferencesDto
    {
        public int UserId { get; set; }
        public int DoctorId { get; set; }
        public int ReferenceId { get; set; }
        public string DoctorName { get; set; }
        public string DoctorSpecialty { get; set; }
        public string Reason { get; set; }
        public string Diagnosis { get; set; }
        public string DoctorSuggestions { get; set; }
        public DateTime ReferenceDate { get; set; }
        public DateTime? FollowUpDate { get; set; }
        public bool IsUsed { get; set; }
        public string Status { get; set; }
        public string ContactDetails { get; set; }
    }
}
