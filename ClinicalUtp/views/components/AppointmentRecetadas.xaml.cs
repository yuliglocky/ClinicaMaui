using System.Collections.ObjectModel;

namespace ClinicalUtp.views.components;
using System.Diagnostics;
using ClinicalUtp.Controllers;
using ClinicalUtp.Models;
public partial class AppointmentRecetadas : ContentView
{

    private readonly AppointmentsServices _appointmentService;

    // Lista de citas que se mostrará en la página
    public ObservableCollection<AppointmentDto> Appointments { get; set; }
    private int _appointmentId; // ID de la cita

    public AppointmentRecetadas(AppointmentsServices appointmentService)
    {
        InitializeComponent();

        _appointmentService = new AppointmentsServices(new HttpClient());
        Appointments = new ObservableCollection<AppointmentDto>();
        BindingContext = this;
        LoadAppointmentsWithStatus0();
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


    private async void LoadAppointmentsWithStatus0()
    {
        try
        {
            var appointments = await _appointmentService.GetAppointmentsWithStatus0();
            if (appointments != null && appointments.Any())
            {
                Appointments.Clear();
                foreach (var appointment in appointments)
                {
                    Appointments.Add(appointment);
                }
            }
            else
            {
                await ShowAlert("Aviso", "No se encontraron citas con estado pendiente.", "OK");
            }
        }
        catch (Exception ex)
        {
            await ShowAlert("Error", $"Error al cargar las citas: {ex.Message}", "OK");
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
    private async void OnApproveButtonClicked(object sender, EventArgs e)
    {
        try
        {
            // Obtener el ID de la cita desde el CommandParameter
            var button = sender as Button;
            if (button?.CommandParameter is int appointmentId)
            {
                bool success = await _appointmentService.ApproveAppointment(appointmentId);
                if (success)
                {
                    var appointmentToRemove = Appointments.FirstOrDefault(a => a.Id == appointmentId);
                    if (appointmentToRemove != null)
                    {
                        Appointments.Remove(appointmentToRemove);
                    }

                    await ShowAlert("Éxito", "La cita ha sido aprobada automáticamente.", "OK");
                }
                else
                {
                    await ShowAlert("Error", "No se pudo aprobar la cita.", "OK");
                }
            }
        }
        catch (Exception ex)
        {
            await ShowAlert("Error", $"Error: {ex.Message}", "OK");
        }
    }

    private async void OnRejectButtonClicked(object sender, EventArgs e)
    {
        try
        {
            // Obtener el ID de la cita desde el CommandParameter
            var button = sender as Button;
            if (button?.CommandParameter is int appointmentId)
            {
                bool success = await _appointmentService.RejectAppointment(appointmentId);
                if (success)
                {
                    var appointmentToRemove = Appointments.FirstOrDefault(a => a.Id == appointmentId);
                    if (appointmentToRemove != null)
                    {
                        Appointments.Remove(appointmentToRemove);
                    }

                    await ShowAlert("Éxito", "La cita ha sido rechazada automáticamente.", "OK");
                }
                else
                {
                    await ShowAlert("Error", "No se pudo rechazar la cita.", "OK");
                }
            }
        }
        catch (Exception ex)
        {
            await ShowAlert("Error", $"Error: {ex.Message}", "OK");
        }
    }

}
