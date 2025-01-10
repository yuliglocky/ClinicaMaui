
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
                    Name = "Consultas y evaluaciones m�dicas",
                    Description = "Evaluaciones m�dicas con previa cita para diagn�sticos y seguimientos."
                },
                new ServiceItem
                {
                    Name = "Consultas de urgencias",
                    Description = "Atenci�n m�dica inmediata para casos de emergencia."
                },
                new ServiceItem
                {
                    Name = "Referencias a especialidades",
                    Description = "Derivaciones a especialistas m�dicos seg�n diagn�stico."
                },
                new ServiceItem
                {
                    Name = "Certificado de buena salud",
                    Description = "Documentaci�n oficial para confirmar buen estado de salud."
                },
                new ServiceItem
                {
                    Name = "Solicitudes de estudios",
                    Description = "Gesti�n para an�lisis de laboratorio o estudios de gabinete."
                },
                new ServiceItem
                {
                    Name = "Administraci�n de medicamentos",
                    Description = "Provisi�n gratuita de medicamentos b�sicos."
                },
                new ServiceItem
                {
                    Name = "Curaciones y retiro de puntos",
                    Description = "Atenci�n para curaciones de heridas y retiro de puntos de sutura."
                },
                new ServiceItem
                {
                    Name = "Control de peso y talla",
                    Description = "Monitoreo y registro de peso y talla para chequeos generales."
                },
                new ServiceItem
                {
                    Name = "Control de presi�n arterial",
                    Description = "Medici�n y monitoreo de la presi�n arterial."
                },
                new ServiceItem
                {
                    Name = "Inhaloterapias",
                    Description = "Tratamientos con inhaladores para condiciones respiratorias."
                },
                new ServiceItem
                {
                    Name = "Aplicaci�n de medicamentos inyectables",
                    Description = "Administraci�n de medicamentos por v�a intramuscular o subcut�nea."
                },
                new ServiceItem
                {
                    Name = "Toma de glicemia capilar",
                    Description = "Medici�n r�pida de los niveles de az�car en sangre."
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


