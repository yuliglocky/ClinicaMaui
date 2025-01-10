using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicalUtp.Models
{
    public class AppointmentDto
    {
        public int Id { get; set; }
        public int UserId { get; set; } // ID del paciente
        public int DoctorId { get; set; } // ID del doctor
        public string Reason { get; set; }
        public DateTime AppointmentDate { get; set; }
        public int Status { get; set; } // Por ejemplo, 0 para pendiente, 1 para confirmado, etc.
        public string Notes { get; set; }
        public bool IsDeleted { get; set; } // Para manejo de eliminación lógica
        public DateTime CreatedAt { get; set; } // Fecha de creación de la cita
        public string BackgroundColor { get; set; }
        public string DoctorName { get; set; }
        public string UserName { get; set; }
        public string DoctorServiceName { get; set; }
  
      

    }


}
