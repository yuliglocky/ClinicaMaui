
using System.Collections.ObjectModel;

namespace ClinicalUtp.viewModels
{
    public partial class ServicesViewModel
    {

            public ObservableCollection<ServiceItem> Services { get; set; }

            public ServicesViewModel()
            {
                Services = new ObservableCollection<ServiceItem>
            {
                new ServiceItem
                {
                    Name = "Consultas y evaluaciones médicas",
                    Description = "Evaluaciones médicas con previa cita para diagnósticos y seguimientos."
                },
                new ServiceItem
                {
                    Name = "Consultas de urgencias",
                    Description = "Atención médica inmediata para casos de emergencia."
                },
                new ServiceItem
                {
                    Name = "Referencias a especialidades",
                    Description = "Derivaciones a especialistas médicos según diagnóstico."
                },
                new ServiceItem
                {
                    Name = "Certificado de buena salud",
                    Description = "Documentación oficial para confirmar buen estado de salud."
                },
                new ServiceItem
                {
                    Name = "Solicitudes de estudios",
                    Description = "Gestión para análisis de laboratorio o estudios de gabinete."
                },
                new ServiceItem
                {
                    Name = "Administración de medicamentos",
                    Description = "Provisión gratuita de medicamentos básicos."
                },
                new ServiceItem
                {
                    Name = "Curaciones y retiro de puntos",
                    Description = "Atención para curaciones de heridas y retiro de puntos de sutura."
                },
                new ServiceItem
                {
                    Name = "Control de peso y talla",
                    Description = "Monitoreo y registro de peso y talla para chequeos generales."
                },
                new ServiceItem
                {
                    Name = "Control de presión arterial",
                    Description = "Medición y monitoreo de la presión arterial."
                },
                new ServiceItem
                {
                    Name = "Inhaloterapias",
                    Description = "Tratamientos con inhaladores para condiciones respiratorias."
                },
                new ServiceItem
                {
                    Name = "Aplicación de medicamentos inyectables",
                    Description = "Administración de medicamentos por vía intramuscular o subcutánea."
                },
                new ServiceItem
                {
                    Name = "Toma de glicemia capilar",
                    Description = "Medición rápida de los niveles de azúcar en sangre."
                }
            };
            }
        }

        public class ServiceItem
        {
            public string Name { get; set; }
            public string Description { get; set; }
        }
    }


