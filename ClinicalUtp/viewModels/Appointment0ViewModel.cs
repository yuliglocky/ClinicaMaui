using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClinicalUtp.Models;
using ClinicalUtp.Controllers;


namespace ClinicalUtp.viewModels
{
    public class Appointment0ViewModel : BaseViewModel
        {
            private readonly AppointmentsServices _appointmentsServices;

            public ObservableCollection<AppointmentDto> Appointments { get; set; }

            // Constructor público sin parámetros
            public Appointment0ViewModel()
            {
                _appointmentsServices = DependencyService.Get<AppointmentsServices>(); // Usamos DependencyService o el mecanismo adecuado para obtener el servicio
                Appointments = new ObservableCollection<AppointmentDto>();
            }

         
        }
    }
