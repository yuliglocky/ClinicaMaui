using System.Diagnostics;
using ClinicalUtp.Controllers;
using ClinicalUtp.Models;
using ClinicalUtp.viewModels;

using Microsoft.Maui.Controls;

namespace ClinicalUtp.views;

public partial class ProfilePage : ContentPage
{
    private readonly PacienteServices _pacienteServices;
    private readonly LoginServices _loginServices;
    private int _userId;
    public int SelectedDoctorId { get; private set; }
    private List<DoctorDto> _doctors;
    private List<ServicioDto> _servicio;

    public ProfilePage(int userId)
    {
        InitializeComponent();
        _userId = userId;
        _pacienteServices = new PacienteServices(new HttpClient());
        LoadDoctors();
        LoadServicio();

        SelectedDoctorId = -1; // Inicializa el SelectedDoctorId a un valor no válido
    }

    private void AnadirClicked(object sender, EventArgs e)
    {
        if (sender is Button clickedButton)
        {
            OnAppointmentView();
        }
    }

    private void OnAppointmentView()
    {
        // Cargar y mostrar la vista de registro
    }

    private async void LoadDoctors()
    {
        try
        {
            _doctors = await _pacienteServices.GetDoctorsBasicAsync();
            if (_doctors != null && _doctors.Count > 0)
            {
                foreach (var doctor in _doctors)
                {
                    doctorPicker.Items.Add(doctor.Name); // Añade solo el nombre al Picker
                }
            }
            else
            {
                await DisplayAlert("Información", "No se encontraron doctores.", "OK");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Ocurrió un error al cargar los doctores: {ex.Message}", "OK");
        }
    }
    private async void LoadServicio()
    {
        try
        {
            _servicio = await _pacienteServices.GetServiciosAsync();
            if (_servicio != null && _servicio.Count > 0)
            {
                // Asegúrate de que el Picker para servicios esté configurado si usas uno aparte
                foreach (var servicio in _servicio)
                {

                }
            }
            else
            {
                await DisplayAlert("Información", "No se encontraron servicios.", "OK");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Ocurrió un error al cargar los servicios: {ex.Message}", "OK");
        }
    }

    private void OnDoctorSelectedIndexChanged(object sender, EventArgs e)
    {
        if (doctorPicker.SelectedIndex != -1)
        {
            var selectedDoctor = _doctors[doctorPicker.SelectedIndex];
            if (selectedDoctor != null)
            {
                SelectedDoctorId = selectedDoctor.Id;
                Console.WriteLine($"Doctor seleccionado ID: {SelectedDoctorId}");
            }
        }
    }
    private async void OnCreateAppointmentClicked(object sender, EventArgs e)
    {
        // Verifica que haya un doctor seleccionado y un motivo para la cita
        if (SelectedDoctorId != -1 && ReasonPicker.SelectedItem != null)
        {
            // Obtiene el motivo de la cita desde el Picker
            string reason = ReasonPicker.SelectedItem.ToString(); // Obtiene el texto del motivo
            DateTime appointmentDate = DateTime.Now.AddDays(2); // Establece la fecha de la cita
            string notes = notesEditor.Text; // Obtiene las notas adicionales

            // Mensaje de confirmación
            string confirmationMessage = $"Confirmar la creación de la cita con los siguientes datos:\n" +
                                         $"UserId: {_userId}\n" +
                                         $"DoctorId: {SelectedDoctorId}\n" +
                                         $"Motivo: {reason}\n" +
                                         $"Fecha de Cita: {appointmentDate}\n" +
                                         $"Notas: {notes}";

            bool isConfirmed = await DisplayAlert("Confirmación de Cita", confirmationMessage, "Confirmar", "Cancelar");

            if (isConfirmed)
            {
                try
                {
                    var appointmentCreateDto = new AppointmentDto
                    {
                        UserId = _userId,
                        DoctorId = SelectedDoctorId,
                        Reason = reason,
                        AppointmentDate = appointmentDate,
                        Notes = notes
                    };

                    var appointmentDto = await _pacienteServices.CreateAppointmentAsync(appointmentCreateDto);

                    if (appointmentDto != null)
                    {
                        Debug.WriteLine($"Cita creada con éxito. ID de la cita: {appointmentDto.Id}");
                        await DisplayAlert("Éxito", "Cita creada correctamente", "OK");
                    }
                    else
                    {
                        await DisplayAlert("Error", "No se pudo crear la cita.", "OK");
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Error al crear la cita: {ex.Message}");
                    await DisplayAlert("Error", "Ocurrió un error al crear la cita.", "OK");
                }
            }
        }
        else
        {
            await DisplayAlert("Advertencia", "Selecciona un doctor y completa el motivo de la cita.", "OK");
        }
    }

}




