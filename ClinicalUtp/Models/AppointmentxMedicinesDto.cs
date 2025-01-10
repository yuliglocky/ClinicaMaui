using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicalUtp.Models
{
    public class AppointmentxMedicinesDto
    {  
            public int Id { get; set; }

            public int UserId { get; set; }
            public int AppointmentId { get; set; }
            public int MedicineId { get; set; }
            public int Quantity { get; set; }
        
    }
}
