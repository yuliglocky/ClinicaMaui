using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicalUtp.Models
{
    public class DoctorDto
    {
        public int Id { get; set; }
        public  int ServicioId { get; set; }
        public string Name { get; set; }
        public string Servicio { get; set; }
    }

    public class ServicioDto
    {
        public int IdServicios { get; set; }
        public string Name { get; set; }
        public bool IsDeleted { get; set; }
        public string Description { get; set; }

    }
}

