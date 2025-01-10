using System.Collections.ObjectModel;
using ClinicalUtp.Controllers;
using ClinicalUtp.Models;
namespace ClinicalUtp.views.components;

public partial class AppointmentAprobada : ContentView
{

    private readonly AppointmentsServices _appointmentService;
    private int _userId;


    // Lista de citas que se mostrará en la página
    public ObservableCollection<AppointmentDto> Appointments { get; set; }
    private int _appointmentId; // ID de la cita
    public AppointmentAprobada(AppointmentsServices appointmentService , int userId)
	{

        _appointmentService = new AppointmentsServices(new HttpClient());
        Appointments = new ObservableCollection<AppointmentDto>();
        BindingContext = this;
        _userId = userId;
        InitializeComponent();
        LoadAppointmentsWithStatus1();

    }



    // Propiedad para mostrar mensajes en la interfaz
    public string Message { get; set; }

    // Método para mostrar un mensaje
    private async Task<bool> ShowAlert(string title, string message, string accept, string cancel = null)
    {
        if (Application.Current.MainPage != null)
        {
            if (cancel == null)
            {
                await Application.Current.MainPage.DisplayAlert(title, message, accept);
                return true;
            }
            else
            {
                return await Application.Current.MainPage.DisplayAlert(title, message, accept, cancel);
            }
        }
        return false;
    }

    private async void LoadAppointmentsWithStatus1()
    {
        try
        {
            var appointments = await _appointmentService.GetAppointmentsWithStatus1();
            if (appointments != null && appointments.Any())
            {
                Appointments.Clear();
                int index = 0; // Índice para determinar el color

                // Ordenamos las citas en orden ascendente (más antiguas primero)
                var orderedAppointments = appointments.OrderBy(a => a.AppointmentDate).ToList();

                foreach (var appointment in orderedAppointments)
                {
                    // Asignamos un color según el índice
                    string color = index >= 6 ? "#198B89" : "#198B89"; // Cambia el color a partir de la séptima cita

                    // Asignamos el color al objeto de cita
                    appointment.BackgroundColor = color;

                    Appointments.Add(appointment);
                    index++;
                }
            }


            else
            {
                await ShowAlert("Aviso", "No se encontraron citas con estado aprobado", "OK");
            }
        }
        catch (Exception ex)
        {
            await ShowAlert("Error", $"Error al cargar las citas: {ex.Message}", "OK");
        }
    }


    private async void OnApproveButtonClicked(object sender, EventArgs e)
    {
        if (sender is Button button && button.CommandParameter is int appointmentId)
        {
            await Navigation.PushModalAsync(new AppointmentViews(_appointmentService, appointmentId, _userId));
        }
    }


    // Evento al seleccionar una cita en la lista
    private void OnAppointmentSelected(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.Count > 0)
        {
            // Obtenemos la cita seleccionada
            var selectedAppointment = (AppointmentDto)e.CurrentSelection[0];

            // Asignamos el ID de la cita seleccionada
            _appointmentId = selectedAppointment.Id;
        }
    }

}

